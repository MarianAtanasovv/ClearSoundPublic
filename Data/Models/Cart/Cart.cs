using System;
using ClearSoundCompany.Data.Models.Products;
using ClearSoundCompany.Models.Products.Enum;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Data.Models.Cart
{
    public class Cart
    {
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required] public string UserId { get; set; }
        [Range(2, 50)] public int Quantity { get; set; }
        [Required] public string ProductId { get; set; }
        public Product Product { get; set; }
        public Color Color { get; set; }
        public bool MixedColor { get; set; } = false;
    }
}