using Models;
using System;
using Bogus;
using OrderService.Dtos;
namespace UnitTesting.Helper
{
    public static class InventoryHelper
    {
        public static Inventory InventoryData()
        {
            Item item = ItemHelper.ItemData();
            return new Faker<Inventory>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.ItemId, item.Id)
                .RuleFor(x => x.Quantity, new Random().Next(1, int.MaxValue))
                .Generate();
        }

        public static InventoryReadDto InventoryReadDto(Inventory inventory)
        {
            return new InventoryReadDto()
            {
                Id = inventory.Id,
                Quantity = inventory.Quantity,
                ItemId = inventory.ItemId
            };
        }
    }
}
