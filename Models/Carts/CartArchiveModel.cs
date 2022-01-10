using System;
using System.ComponentModel.DataAnnotations;
using ClearSoundCompany.Data.Models.Products;
using ClearSoundCompany.Models.Products.Enum;

namespace ClearSoundCompany.Models.Carts
{
    public class CartArchiveModel
    {
        [Required] public string Id { get; set; }

        [Required] public string UserId { get; set; }

        [Required] public int Quantity { get; set; }

        [Required] public string ImageName { get; set; }
        
        [Required] public Product Product { get; set; }

        [Required] public DateTime OrderDate { get; set; }

        [Required] public double Price { get; set; }
        [Required] public double PaintAudition { get; set; }
        [Required] public double MinimumPriceForDiscount { get; set; }
        [Required] public int MinimumProductsQuantityForDiscount { get; set; }
        [Required] public int MaximumPercentageForDiscount { get; set; }
        [Required] public Color Color { get; set; }

        [Required] public bool MixedColor { get; set; }
    }
}