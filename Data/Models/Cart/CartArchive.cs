using System;
using System.ComponentModel.DataAnnotations;
using ClearSoundCompany.Data.Models.Products;
using ClearSoundCompany.Models.Products.Enum;

namespace ClearSoundCompany.Data.Models.Cart
{
    public class CartArchive
    {
        [Required] public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required] public string UserId { get; set; }
        [Range(2, 50)] public int Quantity { get; set; }
        [Required] public string ProductId { get; set; }
        public Product Product { get; set; }
        [Required] public DateTime OrderDate { get; set; }
        [Required] public double Price;
        public Color Color { get; set; }
    }
}