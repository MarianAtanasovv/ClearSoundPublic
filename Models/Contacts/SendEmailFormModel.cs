using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Models.Contacts
{
    public class SendEmailFormModel
    {
        [Required]
        [MinLength(10)]
        public string Message { get; set; }

        [Required]
        [MinLength(4)]
        public string Subject { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(2)]
        public string Name { get; set; }
    }
}