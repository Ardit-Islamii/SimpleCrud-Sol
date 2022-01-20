using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Item
    {
        /// <summary>
        /// The id of the item
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The Name of the item
        /// </summary>
        [Required]
        public string Name { get; set; }
        
        /// <summary>
        /// The price of the item
        /// </summary>
        [Required]
        public decimal Price { get; set; }
    }
}
