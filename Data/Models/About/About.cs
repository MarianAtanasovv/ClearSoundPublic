using System;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Data.Models.About
{
    public class About
    {
        [Required] public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required] [MinLength(10)] public string Description { get; set; }
        [Required] public string Image { get; set; }
    }
}