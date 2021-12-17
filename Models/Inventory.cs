using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
