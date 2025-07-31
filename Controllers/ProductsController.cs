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




                #region  Convert Enum to Dropdown List for the View

                         // 📌 Suppose we have an enum called Category that defines fixed product categories.
        //public enum Category
        //        {
        //            Electronics,
        //            Furniture,
        //            Books
        //        }

        // ✅ To display these categories in a dropdown (for example, in a form),
        // we need to convert the enum values into a list of SelectListItem objects.

        // 🧠 Steps:

        // 1️⃣ Enum.GetValues(typeof(Category))
        //     - This gets all the values from the enum 'Category'.
        //     - The result is: [Category.Electronics, Category.Furniture, Category.Books]

        // 2️⃣ .Cast<Category>()
        //     - Converts the object[] returned from GetValues into a strongly typed List<Category>

        // 3️⃣ .Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() })
        //     - For each category, we create a SelectListItem where:
        //         - Value = the category name as string
        //         - Text = the category name as string (to display in the dropdown)

        // 4️⃣ .ToList()
        //     - Converts the result to a List<SelectListItem>

        #endregion




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

        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            var viewModel = new ProductUpdateViewModel
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                SupplierId = product.SupplierId,
                Suppliers = _context.Suppliers
                    .Select(s => new SelectListItem { Value = s.SupplierId.ToString(), Text = s.Name })
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(ProductUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Suppliers = _context.Suppliers
                    .Select(s => new SelectListItem { Value = s.SupplierId.ToString(), Text = s.Name })
                    .ToList();
                return View(model);
            }

            var product = _context.Products.Find(model.ProductId);
            if (product == null)
                return NotFound();

            product.Name = model.Name;
            product.Price = model.Price;
            product.Category = model.Category;
            product.SupplierId = model.SupplierId;

            try
            {
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while updating: {ex.Message}");
                model.Suppliers = _context.Suppliers
                    .Select(s => new SelectListItem { Value = s.SupplierId.ToString(), Text = s.Name })
                    .ToList();
                return View(model);
            }
        }


        public IActionResult Delete(int id)
        {
            var product = _context.Products.Include(p => p.Supplier).FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            var viewModel = new ProductDeleteViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SupplierName = product.Supplier?.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Delete(ProductDeleteViewModel model)
        {
            var product = _context.Products.Find(model.Id);
            if (product == null)
                return NotFound();

            try
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while deleting: {ex.Message}");
                return View(model);
            }
        }


    }
}
