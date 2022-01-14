using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using Models;

namespace UnitTesting.Helper
{
    public static class PurchaseHelper
    {

        public static Purchase PurchaseData()
        {
            Item item = ItemHelper.ItemData();
            return new Faker<Purchase>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.ItemId, item.Id)
                .RuleFor(x => x.Amount, new Randomizer().Int())
                .Generate();
        }
    }
}
