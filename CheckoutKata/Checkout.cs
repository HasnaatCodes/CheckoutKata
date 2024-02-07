using CheckoutKata.DatabaseContext;
using CheckoutKata.Exceptions;

namespace CheckoutKata;

public class Checkout : ICheckout
{
    private readonly ProductDatabaseContext _dbContext;
    private readonly Dictionary<string, int> _cart = new();
    private decimal _total;
    private decimal _totalSavings; 

    public Checkout(ProductDatabaseContext dbContext)
    {
        _dbContext = dbContext;
        _dbContext.Database.EnsureCreated();
    }
    
    public void Scan(string item)
    {
        var product = _dbContext.Products.FirstOrDefault(x => x.Sku == item);
        if (product == null)
        {
            throw new NotFoundException("Item does not exist.");
        }
        
        AddToCart(product);
    }
    
    public decimal GetTotalPrice()
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

    public decimal GetTotalSavings()
    {
        return _totalSavings;
    }

    private void ApplySpecialPrice(int currentQuantity, Product product)
    {
        var timesDiscountWillBeApplied = currentQuantity / product.QuantityRequiredForSpecialPrice;
        var nonDiscountedQuantity = currentQuantity % product.QuantityRequiredForSpecialPrice;

        var discountedPrice = timesDiscountWillBeApplied * product.SpecialPrice;
        var regularPrice = nonDiscountedQuantity * product.RegularPrice;
        var listedTotalPrice = currentQuantity * product.RegularPrice;
        
        _totalSavings += listedTotalPrice - (discountedPrice + regularPrice);
        _total -= listedTotalPrice;
        _total += discountedPrice + regularPrice;
    }

    private void AddToCart(Product product)
    {
        if (_cart.TryGetValue(product.Sku, out var quantity))
        {
            _cart[product.Sku] = quantity + 1;
        }
        else
        {
            _cart[product.Sku] = 1;
        }
        
        _total += product.RegularPrice;
    }
}