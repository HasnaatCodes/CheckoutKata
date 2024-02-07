using CheckoutKata.DatabaseContext;

namespace CheckoutKata;

public class Checkout : ICheckout
{
    private readonly ProductDatabaseContext _dbContext;
    private readonly Dictionary<string, int> _cart = new();
    private int _total;

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
        _total += productDetails.RegularPrice;
    }
    
    public int GetTotalPrice()
    {
        var currentItemsInCart = _dbContext.Products
            .Where(p => _cart.ContainsKey(p.Sku))
            .ToList();

        foreach (var product in currentItemsInCart)
        {
            var currentQuantity = _cart[product.Sku];
            if (product.QuantityRequiredForSpecialPrice > 0 && currentQuantity >= product.QuantityRequiredForSpecialPrice)
            {
                ApplySpecialPrice(currentQuantity, product);
            }
        }
        
        return _total;
    }

    private void ApplySpecialPrice(int currentQuantity, Product product)
    {
        var timesDiscountWillBeApplied = currentQuantity / product.QuantityRequiredForSpecialPrice;
        var nonDiscountedQuantity = currentQuantity % product.QuantityRequiredForSpecialPrice;

        var discountedPrice = timesDiscountWillBeApplied * product.SpecialPrice;
        var regularPrice = nonDiscountedQuantity * product.RegularPrice;

        _total -= currentQuantity * product.RegularPrice;
        _total += discountedPrice + regularPrice;
    }

    private void AddToCart(string item)
    {
        if (_cart.TryGetValue(item, out var quantity))
        {
            _cart[item] = quantity + 1;
        }
        else
        {
            _cart[item] = 1;
        }
    }
}