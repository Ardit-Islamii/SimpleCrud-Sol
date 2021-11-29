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
                Id = Guid.NewGuid(),
                Name = "TestItem",
                Price = 100.1m
            };
        }
    }
}
