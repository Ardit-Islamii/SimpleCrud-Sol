using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using SimpleCrud.Models;
using SubscriberExample.Contracts.Services;
using System.Threading.Tasks;

namespace SubscriberExample.Consumers
{
    public class PurchaseConsumer : IConsumer<Purchase>
    {
        private readonly ILogger<PurchaseConsumer> _logger;
        private readonly IInventoryService _inventoryService;

        public PurchaseConsumer(ILogger<PurchaseConsumer> logger, IInventoryService inventoryService)
        {
            _logger = logger;
            _inventoryService = inventoryService;
        }
        public async Task Consume(ConsumeContext<Purchase> context)
        {
            _logger.LogInformation($"--> Logging information about {context.Message.ItemId}");
            var result =  await _inventoryService.DecrementItemQuantity(context.Message.ItemId);
            _logger.LogInformation("--> Inventory item amount " + ((result) ? "was decreased by 1": "could not be decreased at all"));
        }
    }
}
