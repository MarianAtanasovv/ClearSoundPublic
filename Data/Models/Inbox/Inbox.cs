using System;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Data.Models.Inbox
{
    public class Inbox
    {
        [Required] public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required] public string Type { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        [Required] public string Message { get; set; }
    }
}