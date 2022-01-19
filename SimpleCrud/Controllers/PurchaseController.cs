using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using Nest;
using OrderService.ClientFactory;
using OrderService.Contracts.Services;
using OrderService.DataAccess;
using OrderService.Dtos;
using OrderService.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderService.Controllers
{
    /// <summary>
    /// The controller for creating purchases.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPublishEndpoint _publish;
        private readonly IPurchaseService _purchaseService;
        private readonly IItemService _itemService;
        private readonly IInventoryClientProvider _inventoryData;
        private readonly ILogger<PurchaseController> _logger;
        private readonly IElasticClient _client;
        private readonly IClientFactory<IInventoryClientProvider> _inventoryClientFactory;
        private readonly IConfiguration _configuration;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public PurchaseController(IPurchaseService purchaseService, IPublishEndpoint publish, IItemService itemService,
            IInventoryClientProvider inventoryData,
            ILogger<PurchaseController> logger,
            IElasticClient client,
            IClientFactory<IInventoryClientProvider> inventoryClientFactory,
            IConfiguration configuration
            )
        {
            _publish = publish;
            _purchaseService = purchaseService;
            _itemService = itemService;
            _inventoryData = inventoryData;
            _logger = logger;
            _client = client;
            _inventoryClientFactory = inventoryClientFactory;
            _configuration = configuration;
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }

        /// <summary>
        /// Creates a purchase based on itemId.
        /// First it gets the quantity available for that item and checks if its more than zero.
        /// If there are no available items, itll send a bad request.
        /// If there are available items it'll create a purchase and publish a message to the InventoryService on the purchaseConsumer class and decrement an item on the inventory there.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST api/<PurchaseController>
        [HttpPost("createpurchase/{id}")]
        public async Task<IActionResult> Post(Guid id)
        {
            var uri = _configuration.GetSection(InventoryOptions.DefaultSection).GetSection("Uri");
            var inventoryClient = await _inventoryClientFactory.CreateClient(uri.Value);

            Item item = await _itemService.Get(id);
            InventoryReadDto inventoryItem = await inventoryClient.GetInventory(id, cancellationToken);
            if (inventoryItem.Quantity > 0)
            {
                Purchase result = await _purchaseService.Create(item);
                try
                {
                    //Publishing using MassTransit
                    await _publish.Publish<Purchase>(result);
                    _logger.LogInformation("--> Published a purchase entity to the InventoryService");

                    //Indexing to Kibana
                    var response = await _client.IndexDocumentAsync(result, cancellationToken);
                    if (response.IsValid)
                    {
                        _logger.LogInformation(response.Id);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "--> Failed to publish message to InventoryService");
                }
                return Ok(result);
            }
            return BadRequest("--> Can't buy a product if its out of stock");
        }
    }
}
