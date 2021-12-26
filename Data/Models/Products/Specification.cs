
namespace ClearSoundCompany.Data.Models.Products
{
    public class Specification
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}