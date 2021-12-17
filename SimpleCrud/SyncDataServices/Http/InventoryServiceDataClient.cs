using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrderService.Dtos;

namespace OrderService.SyncDataServices.Http
{
    public class InventoryServiceDataClient : IInventoryServiceDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public InventoryServiceDataClient(HttpClient httpClient, ILoggerFactory logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger.CreateLogger("InventoryServiceDataClient");
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
                _logger.LogInformation("--> Sync POST to InventoryService was OK!");
            }
            else
            {
                _logger.LogWarning("--> Couldn't sync POST to InventoryService.");
            }
        }
    }
}
