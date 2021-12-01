using System;

namespace SubscriberExample.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public Guid ExternalId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
