using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Areas.Administration.Models.About
{
    public class AddOrEditFormModel
    {
        [Required] [MinLength(10)] public string Description { get; set; }

        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
    }
}