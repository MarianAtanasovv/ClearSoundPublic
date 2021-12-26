using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Data.Models.Products
{
    public class Category
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required] public string Name { get; set; }
        public bool IsAddable { get; set; }
        [Required] public int MinimumQuantity { get; set; }
        public ICollection<Product> Products { get; init; } = new List<Product>();
    }
}