using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Data.Models.Products
{
    public class Product
    {
        [Required] public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required] [MaxLength(50)] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string ProfileImage { get; set; }
        [Required] public string CompressedProfileImage { get; set; }
        public double Price { get; set; }
        public string EaseData { get; set; }
        public string ProductData { get; set; }
        public string Manual { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public bool New { get; set; } = false;
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public ICollection<Specification> Specifications { get; set; } = new List<Specification>();
    }
}