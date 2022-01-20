using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GreenPipes;
using InventoryService.Consumers;
using InventoryService.Contracts.Services;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using Moq;
using Nest;
using OrderService.ClientFactory;
using OrderService.Contracts.Repositories;
using OrderService.Contracts.Services;
using OrderService.DataAccess;
using OrderService.Dtos;
using OrderService.Services;
using UnitTesting.Helper;
using Xunit;
namespace UnitTesting.PurchaseUnitTest
{
    public class PurchaseServiceTest
    {
        // _sut necessary mocks.
        private readonly Mock<IPurchaseRepository> _mockedPurchaseRepository;
        private readonly Mock<IPublishEndpoint> _mockedPublishEndpoint;
        private readonly Mock<IItemService> _mockedItemService;
        private readonly Mock<ILogger<PurchaseService>> _mockedLogger;
        private readonly Mock<IInventoryClientProvider> _mockedInventoryClientProvider;
        private readonly Mock<IClientFactory<IInventoryClientProvider>> _mockedClientFactory;
        private readonly Mock<IConfiguration> _mockedConfiguration;
        private readonly Mock<IConfigurationSection> _mockedConfigurationSection;
        private readonly Mock<IMapper> _mockedMapper;
        private readonly Mock<IElasticClient> _mockedElasticClient;

        // Consumer necessary mocks
        private readonly Mock<IInventoryService> _mockedInventoryService;
        private readonly Mock<ILogger<PurchaseConsumer>> _mockedConsumerLogger;

        private readonly static Item item = ItemHelper.ItemData();
        private readonly static ItemReadDto itemReadDto = ItemHelper.ItemReadDtoData(item);
        private readonly static Inventory inventory = InventoryHelper.InventoryData();
        private readonly static InventoryReadDto inventoryReadDto = InventoryHelper.InventoryReadDto(inventory);
        private readonly static Purchase purchase = PurchaseHelper.PurchaseData(item);
        private readonly static PurchaseReadDto purchaseReadDto = PurchaseHelper.PurchaseReadDtoData(purchase);

        private readonly PurchaseService _sut;


        public PurchaseServiceTest()
        {
            _mockedPurchaseRepository = new Mock<IPurchaseRepository>();
            _mockedPublishEndpoint = new Mock<IPublishEndpoint>();
            _mockedLogger = new Mock<ILogger<PurchaseService>>();
            _mockedConfiguration = new Mock<IConfiguration>();
            _mockedItemService = new Mock<IItemService>();
            _mockedMapper = new Mock<IMapper>();
            _mockedInventoryClientProvider = new Mock<IInventoryClientProvider>();
            _mockedClientFactory = new Mock<IClientFactory<IInventoryClientProvider>>();
            _mockedElasticClient = new Mock<IElasticClient>();
            _sut = new PurchaseService(
                _mockedPublishEndpoint.Object,
                _mockedItemService.Object,
                _mockedLogger.Object,
                _mockedElasticClient.Object, 
                _mockedClientFactory.Object,
                _mockedConfiguration.Object,
                _mockedPurchaseRepository.Object,
                _mockedMapper.Object
                );

            _mockedInventoryService = new Mock<IInventoryService>();
            _mockedConsumerLogger = new Mock<ILogger<PurchaseConsumer>>();
        }

        [Fact]
        public async Task Create_ValidData_CreatedPurchaseAsync()
        {
            //Arrange
            _mockedConfigurationSection.Setup(x => x.Path)
                .Returns("Item:Uri");
            _mockedConfigurationSection.Setup(x => x.Key)
                .Returns("Uri");
            _mockedConfigurationSection.Setup(x => x.Value)
                .Returns("http://localhost:6000/api/c/inventory");
            _mockedConfiguration.Setup(x => x.GetSection(It.IsAny<string>()))
                .Returns(_mockedConfigurationSection.Object);
            
            _mockedInventoryClientProvider.Setup(x => x.GetInventory(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(inventoryReadDto);
            _mockedClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                .ReturnsAsync(_mockedInventoryClientProvider.Object);

            _mockedItemService.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(item);
            _mockedPurchaseRepository.Setup(x => x.Create(It.IsAny<Purchase>()))
                .ReturnsAsync(purchase);
            _mockedMapper.Setup(x => x.Map<PurchaseReadDto>(It.IsAny<Purchase>()))
                .Returns(purchaseReadDto);

            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer<PurchaseConsumer>(() => new PurchaseConsumer(_mockedConsumerLogger.Object, _mockedInventoryService.Object), cfg =>
            {
                cfg.UseMessageRetry(r => r.Incremental(3, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(2)));
            });

            await harness.Start();

            _mockedInventoryService.Setup(x => x.DecrementItemQuantity(It.IsAny<Guid>())).ReturnsAsync(true);

            //Act
            var result = await _sut.Create(item.Id);

            await harness.InputQueueSendEndpoint.Send<Purchase>(purchase);
            //Assert

            // did the endpoint consume the message
            Assert.True(await harness.Consumed.Any<Purchase>());

            // did the actual consumer consume the message
            Assert.True(await consumerHarness.Consumed.Any<Purchase>());

        }
    }
}
