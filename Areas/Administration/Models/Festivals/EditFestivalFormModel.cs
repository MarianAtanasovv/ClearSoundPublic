using ClearSoundCompany.Models.Festivals;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Areas.Administration.Models.Festivals
{
    public class EditFestivalFormModel
    {
        public string Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; init; }

        [MinLength(5)]
        [MaxLength(20)]
        public string Date { get; set; }

        [Display(Name = "Country")] public string CountryId { get; init; }
        public ICollection<FestivalCountryViewModel> Countries { get; set; }
    }
}