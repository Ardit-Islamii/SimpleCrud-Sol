using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using SimpleCrud.Contracts.Services;
using SimpleCrud.DataAccess;
using SimpleCrud.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpleCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPublishEndpoint _publish;
        private readonly IPurchaseService _purchaseService;
        private readonly IItemService _itemService;
        private readonly IInventoryData _inventoryData;
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(IPurchaseService purchaseService, IPublishEndpoint publish, IItemService itemService,
            IInventoryData inventoryData,
            ILogger<PurchaseController> logger
            )
        {
            _publish = publish;
            _purchaseService = purchaseService;
            _itemService = itemService;
            _inventoryData = inventoryData;
            _logger = logger;
        }

        // POST api/<PurchaseController>
        [HttpPost("createpurchase/{id}")]
        public async Task<IActionResult> Post(Guid id)
        {
            Item item = await _itemService.Get(id);
            Inventory inventoryItem = await _inventoryData.GetInventory(id);
            var availableStock = inventoryItem.Quantity > 0;
            if (availableStock)
            {
                Purchase result = await _purchaseService.Create(item);
                try
                {
                    await _publish.Publish<Purchase>(result);
                    _logger.LogInformation("--> Published a purchase entity to the SubscriberExample");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "--> Failed to publish message to SubscriberExample");
                }
                return Ok(result);
            }
            return BadRequest("--> Can't buy a product if its out of stock");
        }
    }
}
