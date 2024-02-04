using CheckoutKata.DatabaseContext;

namespace CheckoutKata;

public class Checkout : ICheckout
{
    private readonly ProductDatabaseContext _dbContext;
    private double _total = 0;

    public Checkout(ProductDatabaseContext dbContext)
    {
        _dbContext = dbContext;
        _dbContext.Database.EnsureCreated();
    }
    
    public void Scan(string item)
    {
        var productDetails = _dbContext.Products.FirstOrDefault(x => x.Sku == item);

        if (productDetails != null)
        {
            _total += productDetails.RegularPrice;
        }
    }

    public double GetTotalPrice()
    {
        return _total;
    }
}