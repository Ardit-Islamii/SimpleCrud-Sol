using System;
using Models;

namespace OrderService.Dtos
{
    public class PurchaseReadDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public int Amount { get; set; }

    }
}
