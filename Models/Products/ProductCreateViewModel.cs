using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Task1.Models;

public class ProductCreateViewModel
{
    [Required(ErrorMessage = "Product name is required.")]
    [Display(Name = "Product Name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10,000.")]
    public decimal Price { get; set; }

    [MaxLength(200, ErrorMessage = "Description must be at most 200 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public Category Category { get; set; }

    [Display(Name = "Stock Quantity")]
    [Range(0, 1000, ErrorMessage = "Stock quantity must be between 0 and 1000.")]
    public int StockQuantity { get; set; }

    // SupplierId is selected from dropdown, required to associate product with a supplier
    public int SupplierId { get; set; }

    #region Ways to Handle IsAvailable without showing it to the user

    // Option 1: Use [ScaffoldColumn(false)] to hide from generated views (like scaffolding)
    // This prevents the field from appearing automatically in Razor Pages or forms.
   // [ScaffoldColumn(false)]
   // public bool IsAvailable { get; set; }

    // Option 2: Simply REMOVE the property from the ViewModel
    // If the user never needs to see or set this value, don’t include it in ProductCreateViewModel at all:
    // Just define IsAvailable in your Product entity only (not the ViewModel).
    // Then in the controller, assign the default value when mapping:
    //var product = new Product
    //{
    //    IsAvailable = true // set manually in controller
    //};

    // Option 3: Use [HiddenInput(DisplayValue = false)] if you still want to include it in the form,
    // but don't want users to see it (⚠ Not recommended for sensitive fields)
    //[HiddenInput(DisplayValue = false)]
   // public bool IsAvailable { get; set; }

    // This generates: <input type="hidden" name="IsAvailable" value="true" />
    // But: User can change it via browser dev tools, so it's NOT secure

    #endregion

    // IsAvailable is handled in code, not visible in the UI
    [ScaffoldColumn(false)]
    public bool IsAvailable { get; set; }


    // Dropdown data binding strategies in ASP.NET Core MVC
    #region Dropdown Binding: Two Common Approaches

    // 🅰️ Option 1: Using ViewBag (Quick & Dynamic)
    // - Populate the dropdown data inside the controller using ViewBag.
    // - Ideal for quick forms or when the ViewModel doesn't need to carry dropdown data.
    // - Example in controller:
    //     ViewBag.Suppliers = new SelectList(_context.Suppliers, "Id", "Name");
    // - Example in Razor view:
    //     @Html.DropDownListFor(model => model.SupplierId, (SelectList)ViewBag.Suppliers, "Select Supplier")

    // 🅱️ Option 2: Using ViewModel Properties (Structured & Reusable)
    // - Define dropdown lists as properties in your ViewModel (preferably List<SelectListItem> or SelectList).
    // - Recommended for larger apps and strongly-typed binding.
    // - Example in ViewModel:
    //     public List<SelectListItem>? Suppliers { get; set; }
    //     public List<SelectListItem>? Categories { get; set; }
    // - Populate them in the controller:
    //     model.Suppliers = _context.Suppliers
    //         .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
    //         .ToList();
    // - In the Razor view:
    //     @Html.DropDownListFor(model => model.SupplierId, Model.Suppliers, "Select Supplier")

    #endregion

    public List<SelectListItem>? Suppliers { get; set; }
    public List<SelectListItem>? Categories { get; set; }
}
