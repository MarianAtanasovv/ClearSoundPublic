using ClearSoundCompany.Data.Models.Products;
using System.Collections.Generic;

namespace ClearSoundCompany.Models.Products
{
    public class ProductInformationView
    {
        public string Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProfileImage { get; set; }
        public double Price { get; set; }
        public string EaseData { get; set; }
        public string ProductData { get; set; }
        public string Manual { get; set; }
        public bool New { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public string ImageName { get; set; }
        public string Extension { get; set; }
        public ICollection<string> Colors { get; set; } = new List<string>();
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public ICollection<Specification> Specifications { get; set; } = new List<Specification>();
    }
}