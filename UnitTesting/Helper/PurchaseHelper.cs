using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using Models;
using OrderService.Dtos;

namespace UnitTesting.Helper
{
    public static class PurchaseHelper
    {

        public static Purchase PurchaseData(Item item)
        {
            return new Faker<Purchase>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.ItemId, item.Id)
                .RuleFor(x => x.Amount, new Randomizer().Int())
                .Generate();
        }
        public static PurchaseReadDto PurchaseReadDtoData(Purchase purchase)
        {
            return new PurchaseReadDto()
            {
                Id = purchase.Id,
                Item = purchase.Item,
                ItemId = purchase.ItemId
            };
        }
    }
}
