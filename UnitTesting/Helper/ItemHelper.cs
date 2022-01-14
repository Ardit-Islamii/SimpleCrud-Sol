using Models;
using System;
using System.Collections.Generic;
using System.Text;
using Bogus;

namespace UnitTesting.Helper
{
    public static class ItemHelper
    {
        public static Item ItemData()
        {
            return new Faker<Item>().RuleFor(x => x.Id, x => Guid.NewGuid())
                .RuleFor(x => x.Name, x => x.Commerce.Product())
                .RuleFor(x => x.Price, x => Decimal.Parse(x.Commerce.Price()))
                .Generate();
        }
    }
}
