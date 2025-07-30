using System.ComponentModel.DataAnnotations;

namespace Task1.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [EmailAddress]
        public string ContactEmail { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
