using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Nest;
using OrderService.Contracts.Services;
using OrderService.DataAccess;
using OrderService.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderService.Controllers
{
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
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        

        public PurchaseController(IPurchaseService purchaseService, IPublishEndpoint publish, IItemService itemService,
            IInventoryClientProvider inventoryData,
            ILogger<PurchaseController> logger,
            IElasticClient client
            )
        {
            _publish = publish;
            _purchaseService = purchaseService;
            _itemService = itemService;
            _inventoryData = inventoryData;
            _logger = logger;
            _client = client;
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }

        // POST api/<PurchaseController>
        [HttpPost("createpurchase/{id}")]
        public async Task<IActionResult> Post(Guid id)
        {
            Item item = await _itemService.Get(id);
            InventoryReadDto inventoryItem = await _inventoryData.GetInventory(id, cancellationToken);
            var availableStock = inventoryItem.Quantity > 0;
            if (availableStock)
            {
                Purchase result = await _purchaseService.Create(item);
                try
                {
                    await _publish.Publish<Purchase>(result);
                    _logger.LogInformation("--> Published a purchase entity to the InventoryService");

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
