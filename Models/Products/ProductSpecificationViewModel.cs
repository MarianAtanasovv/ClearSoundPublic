using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Models.Products
{
    public class ProductSpecificationViewModel
    {
        [Required] [MinLength(3)] public string SpecificationName { get; set; }

        [Required] [MinLength(3)] public string SpecificationDescription { get; set; }
    }
}