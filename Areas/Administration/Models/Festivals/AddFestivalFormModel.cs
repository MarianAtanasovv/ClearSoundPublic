using ClearSoundCompany.Models.Festivals;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Areas.Administration.Models.Festivals
{
    public class AddFestivalFormModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; init; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Date { get; set; }

        [Required] public IFormFileCollection Images { get; set; }

        [Display(Name = "Country")] public string CountryId { get; init; }
        public ICollection<FestivalCountryViewModel> Countries { get; set; }
    }
}