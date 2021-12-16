using Models;
using Moq;
using SimpleCrud.Contracts.Repositories;
using SimpleCrud.Models;
using SimpleCrud.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnitTesting.Helper;
using Xunit;

namespace UnitTesting.ItemUnitTest
{
    public class ItemServiceTest
    {
        private readonly Mock<IItemRepository> _mockedItemRepository;
        private readonly ItemService _itemService;
        private static Item item = ItemHelper.ItemData();

        public ItemServiceTest()
        {
            _mockedItemRepository = new Mock<IItemRepository>();
            _itemService = new ItemService(_mockedItemRepository.Object);
        }
        [Fact]
        public async Task Create_ValidData_CreatedItem()
        {
            //Arrange
            _mockedItemRepository.Setup(x => x.Create(It.IsAny<Item>())).ReturnsAsync(item);

            //Act
            var result = await _itemService.Create(item);

            //Assert
            Assert.Equal(result, item);
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
        public async Task DeleteItemAsync_NonexistingItem_ItemNotFound()
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
