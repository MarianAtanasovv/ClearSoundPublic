using System;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Data.Models.Rental
{
    public class FestivalImage
    {
        [Required] public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required] public string Name { get; set; }
        public string FestivalId { get; set; }
        public Festival Festival { get; init; }
    }
}