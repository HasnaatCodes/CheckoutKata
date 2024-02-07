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

            if (product.QuantityRequiredForSpecialPrice == 0 || currentQuantity < product.QuantityRequiredForSpecialPrice)
            {
                continue;
            }
            
            var timesDiscountWillBeApplied = currentQuantity / product.QuantityRequiredForSpecialPrice;
            
            if (currentQuantity % product.QuantityRequiredForSpecialPrice == 0)
            {
                // Subtract all of this product's total from the total currently
                _total -= currentQuantity * product.RegularPrice;
            }
            else
            {
                // Subtract the total of number of items currently in cart that will be discounted
                var quantityBeingDiscounted = timesDiscountWillBeApplied * product.QuantityRequiredForSpecialPrice;
                _total -= quantityBeingDiscounted * product.RegularPrice;
            }
            
            _total += product.SpecialPrice * timesDiscountWillBeApplied;
        }
        
        return _total;
    }

    private void AddToCart(string item)
    {
        if (_cart.ContainsKey(item))
        {
            _cart[item]++;
        }
        else
        {
            _cart.Add(item, 1);
        }
    }
}