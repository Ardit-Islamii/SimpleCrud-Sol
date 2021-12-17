using System;

namespace InventoryService.Dtos
{
    public class InventoryReadDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
