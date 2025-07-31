using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Task1.Models.Products
{
    public class ProductUpdateViewModel
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public int SupplierId { get; set; }

        public List<SelectListItem> Suppliers { get; set; }
    }

}
