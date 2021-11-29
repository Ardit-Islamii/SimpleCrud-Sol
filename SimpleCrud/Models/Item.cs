using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleCrud.Models
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
