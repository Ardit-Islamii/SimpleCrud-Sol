using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimpleCrud.Dtos;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleCrud.SyncDataServices.Http
{
    public class HttpSubscriberExampleDataClient : ISubscriberExampleDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public HttpSubscriberExampleDataClient(HttpClient httpClient, ILoggerFactory logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger.CreateLogger("HttpSubExampleDataClient");
            _configuration = configuration;
        }
        public async Task SendItemToSubExample(ItemReadDto item)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(item),
                Encoding.UTF8,
                "application/json"
                );
            var response = await _httpClient.PostAsync($"{_configuration["SubscriberExample"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("--> Sync POST to SubscriberExample was OK!");
            }
            else
            {
                _logger.LogWarning("--> Couldn't sync POST to SubscriberExample.");
            }
        }
    }
}
