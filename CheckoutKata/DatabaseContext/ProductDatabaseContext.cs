using Microsoft.EntityFrameworkCore;

namespace CheckoutKata.DatabaseContext;

public class ProductDatabaseContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "ProductDb");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasKey(p => p.Sku);
        
        modelBuilder.Entity<Product>().HasData(
            new Product { Sku = "A", RegularPrice = 50, SpecialPrice = 130, QuantityRequiredForSpecialPrice = 3 },
            new Product { Sku = "B", RegularPrice = 30, SpecialPrice = 45, QuantityRequiredForSpecialPrice = 2 },
            new Product { Sku = "C", RegularPrice = 20},
            new Product { Sku = "D", RegularPrice = 15}
        );
    }
}