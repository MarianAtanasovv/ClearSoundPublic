using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClearSoundCompany.Models.Carts
{
    public class CartViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string SelectedColor { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required]
        public double Price { get; set; }

        public ICollection<string> Colors { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}