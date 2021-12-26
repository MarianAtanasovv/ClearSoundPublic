using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Areas.Administration.Models.Products
{
    public class EditCategoryFormModel
    {
        public string Id { get; set; }
        [Required] public string Name { get; set; }
        public bool IsAddable { get; set; }
        [Required] public int MinimumQuantity { get; set; }
    }
}