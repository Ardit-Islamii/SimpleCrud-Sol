using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using Nest;
using OrderService.ClientFactory;
using OrderService.Contracts.Repositories;
using OrderService.Contracts.Services;
using OrderService.Controllers;
using OrderService.DataAccess;
using OrderService.Dtos;
using OrderService.Options;

namespace OrderService.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPublishEndpoint _publish;
        private readonly IItemService _itemService;
        private readonly ILogger<PurchaseController> _logger;
        private readonly IElasticClient _client;
        private readonly IClientFactory<IInventoryClientProvider> _inventoryClientFactory;
        private readonly IConfiguration _configuration;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public PurchaseService(IPublishEndpoint publish,
            IItemService itemService,
            ILogger<PurchaseController> logger,
            IElasticClient client,
            IClientFactory<IInventoryClientProvider> inventoryClientFactory,
            IConfiguration configuration,
            IPurchaseRepository purchaseRepository
        )
        {
            _publish = publish;
            _itemService = itemService;
            _logger = logger;
            _client = client;
            _inventoryClientFactory = inventoryClientFactory;
            _configuration = configuration;
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<Purchase> Create(Guid id)
        {
            var uri = _configuration.GetSection(InventoryOptions.DefaultSection).GetSection("Uri");
            var inventoryClient = await _inventoryClientFactory.CreateClient(uri.Value);

            Item item = await _itemService.Get(id);
            InventoryReadDto inventoryItem = await inventoryClient.GetInventory(id, cancellationToken);

            Purchase purchase = new Purchase()
            {
                ItemId = item.Id,
                Amount = 1
            };

            var result = await _purchaseRepository.Create(purchase);
            if (result == null)
            {
                _logger.LogInformation("Couldn't create a purchase entity on the database");
                return null;
            }

            _logger.LogInformation("Successfully created a purchase entity on the database");

            if (inventoryItem.Quantity > 0)
            {
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

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "--> Failed to publish message to InventoryService");
                    throw;
                }
            }
            return null;
        }
    }
}
