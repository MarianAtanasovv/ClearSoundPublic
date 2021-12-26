using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Data.Models.Rental
{
    public class Country
    {
        [Required] public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required] public string Name { get; set; }
        public string FlagImage { get; set; }
        public virtual ICollection<Festival> Festivals { get; } = new List<Festival>();
    }
}