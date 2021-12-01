using System;

namespace SubscriberExample.Dtos
{
    public class ItemPublishedDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Event { get; set; }
    }
}
