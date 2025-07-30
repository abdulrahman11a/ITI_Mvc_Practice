using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task1.Data;
using Task1.Models;
using Task1.Models.Products;
namespace Task1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string? name, int? supplierId)
        {
            var query = _context.Products
                .Include(p => p.Supplier)
                .Where(p => p.IsAvailable);

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (supplierId.HasValue)
            {
                query = query.Where(p => p.SupplierId == supplierId);
            }

            return View(query.ToList());
        }


        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest("Invalid product ID.");

            var product = _context.Products
                .Include(p => p.Supplier)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound("Product not found.");

            var viewModel = new ProductDetailsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Category = product.Category.ToString(),
                StockQuantity = product.StockQuantity,
                SupplierName = product.Supplier.Name,
                IsAvailable = product.IsAvailable
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new ProductCreateViewModel
            {
                Suppliers = _context.Suppliers
                    .Select(s => new SelectListItem { Value = s.SupplierId.ToString(), Text = s.Name })
                    .ToList(),

                Categories = Enum.GetValues(typeof(Category))
                    .Cast<Category>()
                    .Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() })
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Add(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Suppliers = _context.Suppliers
                    .Select(s => new SelectListItem { Value = s.SupplierId.ToString(), Text = s.Name })
                    .ToList();

                model.Categories = Enum.GetValues(typeof(Category))
                    .Cast<Category>()
                    .Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() })
                    .ToList();

                TempData["Error"] = "Failed to add product. Please fix the errors.";
                return View(model);
            }

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                Category = model.Category,
                SupplierId = model.SupplierId,
                StockQuantity = model.StockQuantity,
                IsAvailable = true
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            TempData["Success"] = "Product added successfully!";
            return RedirectToAction("Index");
        }



    }
}
