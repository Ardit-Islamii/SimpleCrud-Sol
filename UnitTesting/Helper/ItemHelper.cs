using Models;
using System;
using Bogus;
using Microsoft.Extensions.Configuration;
using OrderService.Dtos;

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
        public static ItemReadDto ItemReadDtoData(Item item)
        {
            return new ItemReadDto
            {
                Id = item.Id, 
                Name = item.Name, 
                Price = item.Price
            };
        }
    }
}
