using System.Collections.Generic;

namespace ClearSoundCompany.Models.Carts
{
    public class OrderCartArchiveViewModel
    {
        public ICollection<CartArchiveModel> Products { get; set; } = new List<CartArchiveModel>();
        public double TotalPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double DiscountPercentage { get; set; }
        public double PaintingAddition { get; set; }
    }
}