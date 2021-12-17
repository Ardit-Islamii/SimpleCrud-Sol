using Models;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTesting.Helper;
namespace UnitTesting.Helper
{
    public static class InventoryHelper
    {
        public static Inventory InventoryData()
        {
            Item item = ItemHelper.ItemData();
            var inventory = new Inventory()
            {
                Id = Guid.Parse("a2831248-c609-413a-a4b8-7f37cdb3450a"),
                ItemId = item.Id,
                Quantity = 10
            };
            return inventory;
        } 
    }
}
