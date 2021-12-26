using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Data.Models.Rental
{
    public class Festival
    {
        [Required] public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required] public string Name { get; set; }
        [Required] public DateTime Date { get; set; }
        public string CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<FestivalImage> FestivalImages { get; set; } = new List<FestivalImage>();
    }
}