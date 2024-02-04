using CheckoutKata.DatabaseContext;

namespace CheckoutKata;

public class Checkout : ICheckout
{
    private readonly ProductDatabaseContext _dbContext;
    private int _total = 0;

    private Dictionary<string, int> cart = new Dictionary<string, int>();

    public Checkout(ProductDatabaseContext dbContext)
    {
        _dbContext = dbContext;
        _dbContext.Database.EnsureCreated();
    }
    
    public void Scan(string item)
    {
        var productDetails = _dbContext.Products.FirstOrDefault(x => x.Sku == item);

        if (productDetails == null)
        {
            return;
        }
        
        AddToCart(item);

        if (productDetails.SpecialPrice > 0 && cart[item] == productDetails.QuantityRequiredForSpecialPrice)
        {
            ApplySpecialPrice(productDetails, item);
            return;
        }
        
        _total += productDetails.RegularPrice;
    }
    
    public int GetTotalPrice()
    {
        return _total;
    }

    private void ApplySpecialPrice(Product product, string item)
    {
        _total -= product.RegularPrice * (cart[item] - 1);
        _total += product.SpecialPrice;
    }

    private void AddToCart(string item)
    {
        if (cart.ContainsKey(item))
        {
            cart[item]++;
        }
        else
        {
            cart.Add(item, 1);
        }
    }
}