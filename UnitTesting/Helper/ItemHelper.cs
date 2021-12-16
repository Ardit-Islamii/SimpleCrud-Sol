using Models;
using SimpleCrud.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTesting.Helper
{
    public static class ItemHelper
    {
        public static Item ItemData()
        {
            return new Item()
            {
                Id = Guid.Parse("238b13ad-30e6-4e89-8a70-53071d994255"),
                Name = "TestItem",
                Price = 100.1m
            };
        }
    }
}
