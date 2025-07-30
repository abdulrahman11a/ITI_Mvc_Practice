using System.ComponentModel.DataAnnotations;
namespace Task1.Models
{
    public enum Category
    {
        Electronics,
        Books,
        Accessories,
        Clothing,
        Furniture
    }

    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        public Category Category { get; set; }

        public bool IsAvailable { get; set; }

        public int StockQuantity { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}
