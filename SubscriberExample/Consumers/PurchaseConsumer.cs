using System;
using System.Threading.Tasks;
using InventoryService.Contracts.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using Models;

namespace InventoryService.Consumers
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
            if (context.GetRetryCount() == 0)
            {
                _logger.LogInformation("First time logging, rejected.");
                throw new ArgumentNullException("Test argument null exception 1");
            }

            if (context.GetRetryCount() == 1)
            {
                _logger.LogInformation("Second time logging, rejected.");
                throw new ArgumentNullException("Test argument null exception 2 ");
            }

            _logger.LogInformation($"--> Successfully consumed purchase on item: {context.Message.ItemId}");
            bool result = await _inventoryService.DecrementItemQuantity(context.Message.ItemId);
            _logger.LogInformation("--> Item amount " + ((result) ? "was decreased by 1" : "could not be decreased at all"));
        }
    }
}
