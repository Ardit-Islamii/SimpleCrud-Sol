using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Inventory
    {
        /// <summary>
        /// The id of the inventory
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The foreign key of the itemId
        /// </summary>
        public Guid ItemId { get; set; }
        /// <summary>
        /// The amount of items that are available
        /// </summary>
        public int Quantity { get; set; }
    }
}
