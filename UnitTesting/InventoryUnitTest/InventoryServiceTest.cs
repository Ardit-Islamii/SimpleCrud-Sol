using Moq;
using SubscriberExample.Contracts.Repositories;
using SubscriberExample.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using UnitTesting.Helper;
using Models;

namespace UnitTesting.InventoryUnitTest
{
    public class InventoryServiceTest
    {
        private readonly Mock<IInventoryRepository> _mockedInventoryRepository;
        private readonly InventoryService _sut;
        Inventory inventory = InventoryHelper.InventoryData();
        Item item = ItemHelper.ItemData();
        public InventoryServiceTest()
        {
            _mockedInventoryRepository = new Mock<IInventoryRepository>();
            _sut = new InventoryService(_mockedInventoryRepository.Object);
        }
        [Fact]
        public async void DecrementItemQuantity_MoreThanZeroQuantity_ItemQuantityDecreased()
        {
            //Arrange
            var expectedResult = inventory.Quantity - 1;
            _mockedInventoryRepository.Setup(x => x.DecrementItemQuantity(It.IsAny<Inventory>())).ReturnsAsync(true);
            _mockedInventoryRepository.Setup(x => x.FindByItemID(It.IsAny<Guid>())).ReturnsAsync(inventory);

            //Act
            var result = await _sut.DecrementItemQuantity(item.Id);

            //Assert
            Assert.True(result);
            Assert.Equal(expectedResult, inventory.Quantity);
        }

        [Fact]
        public async void DecrementItemQuantity_ZeroQuantity_ItemQuantityCouldNotBeDecreased()
        {
            //Arrange
            inventory.Quantity = 0;
            var expectedResult = inventory.Quantity;
            _mockedInventoryRepository.Setup(x => x.DecrementItemQuantity(It.IsAny<Inventory>())).ReturnsAsync(false);
            _mockedInventoryRepository.Setup(x => x.FindByItemID(It.IsAny<Guid>())).ReturnsAsync(inventory);

            //Act
            var result = await _sut.DecrementItemQuantity(item.Id);

            //Assert
            Assert.False(result);
            Assert.Equal(expectedResult, inventory.Quantity);
        }
    }
}
