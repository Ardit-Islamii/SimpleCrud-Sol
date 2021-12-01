using System;

namespace SimpleCrud.Dtos
{
    public class ItemReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Event{ get;set; }
    }
}
