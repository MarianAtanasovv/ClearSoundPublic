using System;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Data.Models.Products
{
    public class ProductImage
    {
        [Required] public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required] public string Name { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; init; }
    }
}