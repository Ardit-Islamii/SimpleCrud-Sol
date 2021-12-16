/*
 
    Commented out due to not knowing how to implement it correctly

using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleCrud.AsyncDataServices
{
    public class KafkaProducerBackgroundService :  BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IProducer<Null, string> _producer;

        public KafkaProducerBackgroundService(ILoggerFactory logger, IConfiguration config)
        {
            _logger = logger.CreateLogger("KafkaProducerLogger");
            _config = config;
            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092"
            };
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            for(var i = 0; i < 100; i++)
            {
                var value = $"Hello World {i}";
                _logger.LogInformation(value);
                await _producer.ProduceAsync("demo", new Message<Null, string>()
                {
                    Value = value
                }, stoppingToken);
            }
            _producer.Flush(TimeSpan.FromSeconds(10));
            throw new System.NotImplementedException();
        }
    }
}
*/