using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrderService.Dtos;
using RabbitMQ.Client;

namespace OrderService.AsyncDataServices
{
    /* UNUSED CODE, TRANSFERRED TO MASSTRANSIT. */
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration, ILoggerFactory logger)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            _logger = logger.CreateLogger("MessageBusLogger");

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                _logger.LogInformation("Connected to message bus");
            } catch (Exception ex)
            {
                _logger.LogWarning($"Couldn't connect to the message bus. --> Error: {ex.Message}", ex);
            }
        }
        public void PublishNewItem(ItemReadDto item)
        {
            var message = JsonSerializer.Serialize(item);

            if (_connection.IsOpen)
            {
                _logger.LogInformation("--> RabbitMQ connection open, sending message...");
                SendMessage(message);
            }
            else
            {
                _logger.LogWarning("--> RabbitMQ connection closed, not sending");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
            _logger.LogInformation($"--> We have sent {message}", message);
        }

        public void Dispose()
        {
            _logger.LogInformation("--> MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
        
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogWarning("--> RabbitMQ connection is down");
        }
    }
}
