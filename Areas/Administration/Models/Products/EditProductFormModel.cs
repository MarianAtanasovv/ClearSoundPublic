using ClearSoundCompany.Models.Products;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Areas.Administration.Models.Products
{
    public class EditProductFormModel
    {
        public string Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; init; }

        [Required] [MinLength(10)] public string Description { get; set; }
        public IFormFileCollection Images { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile ProfileImage { get; set; }
        
        [Display(Name = "Compressed Profile Image")]
        public IFormFile CompressedProfileImage { get; set; }

        [Display(Name = "Product Data")] public IFormFile ProductData { get; set; }
        [Display(Name = "EASE Data")] public IFormFile EASEData { get; set; }
        public IFormFile Manual { get; set; }
        public double Price { get; set; }
        public bool New { get; set; }
        [Display(Name = "Category")] public string CategoryId { get; init; }
        public ICollection<ProductCategoryViewModel> Categories { get; set; }
        public ICollection<ProductSpecificationViewModel> Specifications { get; set; }
    }
}