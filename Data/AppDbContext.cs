using Microsoft.EntityFrameworkCore;
using Task1.Models;

namespace Task1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region DbSets
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Supplier Seed
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { SupplierId = 1, Name = "TechCorp", ContactEmail = "tech@corp.com" },
                new Supplier { SupplierId = 2, Name = "BookWorld", ContactEmail = "contact@bookworld.com" }
            );
            #endregion

            #region Product Seed
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 999, Category = Category.Electronics, IsAvailable = true, StockQuantity = 20, SupplierId = 1 },
                new Product { Id = 2, Name = "Keyboard", Price = 50, Category = Category.Accessories, IsAvailable = false, StockQuantity = 10, SupplierId = 1 },
                new Product { Id = 3, Name = "C# Programming Book", Price = 30, Category = Category.Books, IsAvailable = true, StockQuantity = 15, SupplierId = 2 },
                new Product { Id = 4, Name = "Notebook", Price = 5, Category = Category.Books, IsAvailable = true, StockQuantity = 100, SupplierId = 2 }
            );
            #endregion
        }
        #endregion
    }
}
