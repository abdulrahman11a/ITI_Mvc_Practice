using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace Task1.Models.Products
{
    public class ProductDetailsViewModel
    {
        // Product ID is hidden from the user
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        // Only this property from the Supplier should be shown
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        // Hidden from the user
        [ScaffoldColumn(false)]
        public bool IsAvailable { get; set; }
    }
}
