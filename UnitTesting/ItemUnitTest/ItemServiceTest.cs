using Models;
using Moq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrderService.ClientFactory;
using OrderService.Contracts.Repositories;
using OrderService.Dtos;
using OrderService.Providers;
using OrderService.Services;
using UnitTesting.Helper;
using Xunit;

namespace UnitTesting.ItemUnitTest
{
    public class ItemServiceTest
    {
        private readonly Mock<ILogger<ItemService>> _mockedLogger;
        private readonly Mock<IMapper> _mockedMapper;
        private readonly Mock<IConfiguration> _mockedConfiguration;
        private readonly Mock<IClientFactory<IItemClientProvider>> _mockedClientFactoryMock;
        private readonly Mock<IItemClientProvider> _mockedItemClientProvider;
        private readonly Mock<IItemRepository> _mockedItemRepository;
        private readonly Mock<IDistributedCache> _mockedCache;
        private readonly Mock<IConfigurationSection> _mockedConfigurationSection;
        private readonly ItemService _itemService;
        private static Item item = ItemHelper.ItemData();
        private static ItemReadDto itemReadDto = ItemHelper.ItemReadDtoData(item);

        public ItemServiceTest()
        {
            _mockedItemRepository = new Mock<IItemRepository>();
            _mockedLogger = new Mock<ILogger<ItemService>>();
            _mockedMapper = new Mock<IMapper>();
            _mockedConfiguration = new Mock<IConfiguration>();
            _mockedClientFactoryMock = new Mock<IClientFactory<IItemClientProvider>>();
            _mockedItemClientProvider = new Mock<IItemClientProvider>();
            _mockedConfigurationSection = new Mock<IConfigurationSection>();
            _mockedCache = new Mock<IDistributedCache>();

            _itemService = new ItemService(_mockedLogger.Object, 
                _mockedMapper.Object, 
                _mockedCache.Object, 
                _mockedConfiguration.Object, 
                _mockedClientFactoryMock.Object, 
                _mockedItemRepository.Object);
        }
        [Fact]
        public async Task Create_ValidData_CreatedItem()
        {
            //Arrange
            _mockedItemRepository.Setup(x => x.Create(It.IsAny<Item>()))
                    .ReturnsAsync(item);
            _mockedMapper.Setup(x => x.Map<ItemReadDto>(It.IsAny<Item>()))
                .Returns(itemReadDto);
            
            _mockedConfigurationSection.Setup(x => x.Path)
                .Returns("Inventory:Uri");
            _mockedConfigurationSection.Setup(x => x.Key)
                .Returns("Uri");
            _mockedConfigurationSection.Setup(x => x.Value)
                .Returns("http://localhost:6000/api/c/item");
            _mockedConfiguration.Setup(x => x.GetSection(It.IsAny<string>()))
                .Returns(_mockedConfigurationSection.Object);

            _mockedItemClientProvider.Setup(x => x.TestInboundConnection(It.IsAny<ItemReadDto>()))
                .ReturnsAsync(itemReadDto);
            _mockedClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                .ReturnsAsync(_mockedItemClientProvider.Object);
            
            _mockedItemRepository.Setup(x => x.Create(It.IsAny<Item>()))
                .ReturnsAsync(item);

            //Act
            var result = await _itemService.Create(item);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result, itemReadDto);
            _mockedMapper.Verify(x => x.Map<ItemReadDto>(It.IsAny<Item>()), Times.Once);
        }

        [Fact]
        public async Task GetItemByIdAsync_ExistingItem_ItemReturned()
        {
            //Arrange
            _mockedItemRepository.Setup(x => x.Get(item.Id)).ReturnsAsync(item);

            //Act
            var result = await _itemService.Get(item.Id);

            //Assert
            Assert.Equal(result, item);
        }
        [Fact]
        public async Task DeleteItemAsync_ExistingItem_ItemDeleted()
        {
            //Arrange
            _mockedItemRepository.Setup(x => x.Delete(item)).ReturnsAsync(true);
            _mockedItemRepository.Setup(x => x.Get(item.Id)).ReturnsAsync(item);


            //Act
            var result = await _itemService.Delete(item.Id);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteItemAsync_NonExistingItem_ItemNotFound()
        {
            //Arrange
            _mockedItemRepository.Setup(x => x.Delete(item)).ReturnsAsync(false);
            _mockedItemRepository.Setup(x => x.Get(item.Id)).ReturnsAsync((Item)null);


            //Act
            var result = await _itemService.Delete(item.Id);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateItemAsync_ValidData_ItemUpdated()
        {
            //Arrange
            _mockedItemRepository.Setup(x => x.Update(item)).ReturnsAsync(item);
            Item formerItem = ItemHelper.ItemData();
            formerItem.Name = "DifferentName";
            _mockedItemRepository.Setup(x => x.Get(item.Id)).ReturnsAsync(formerItem);

            //Act
            var result = await _itemService.Update(item);

            //Assert
            Assert.NotEqual(result.Name, formerItem.Name);
        }

        [Fact]
        public async Task UpdateItemAsync_ValidData_ItemNotFound()
        {
            //Arrange
            _mockedItemRepository.Setup(x => x.Get(item.Id)).ReturnsAsync((Item)null);
            _mockedItemRepository.Setup(x => x.Update(item)).ReturnsAsync((Item)null);
            
            //Act
            var result = await _itemService.Update(item);

            //Assert
            Assert.Null(result);
        }
    }
}
