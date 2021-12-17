using System;

namespace Models
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public int Amount { get; set; }
    }
}
