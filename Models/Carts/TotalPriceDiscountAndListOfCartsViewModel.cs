using System.Collections.Generic;

namespace ClearSoundCompany.Models.Carts
{
    public class TotalPriceDiscountAndListOfCartsViewModel
    {
        public List<CartViewModel> Products { get; set; }
        public double TotalPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double DiscountPercentage { get; set; }
        public double PaintingAddition { get; set; }
    }
}