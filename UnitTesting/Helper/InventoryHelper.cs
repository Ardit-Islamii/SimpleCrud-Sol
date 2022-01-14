using Models;
using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using UnitTesting.Helper;
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
    }
}
