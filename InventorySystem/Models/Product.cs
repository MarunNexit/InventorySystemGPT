﻿using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Range(0, 9999)]
        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
