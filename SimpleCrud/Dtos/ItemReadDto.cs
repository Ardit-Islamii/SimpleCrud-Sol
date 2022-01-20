using System;

namespace OrderService.Dtos
{
    public class ItemReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
