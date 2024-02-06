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

        // var currentQuantity = _cart[item];
        // if (currentQuantity >= productDetails.QuantityRequiredForSpecialPrice)
        // {
        //     ApplySpecialPrice(productDetails);
        //     return;
        // }
        
        _total += productDetails.RegularPrice;
    }
    
    public int GetTotalPrice()
    {
        // Work out special prices 
        List<string> currentItemsInCart = _cart.Keys.ToList();

        foreach (var item in currentItemsInCart)
        {
            var currentQuantity = _cart[item];
            var productDetail = _dbContext.Products.FirstOrDefault(x => x.Sku == item);
            
            // if there is a special offer we want to work out the special price
            if (productDetail?.QuantityRequiredForSpecialPrice > 0 && _cart[item] >= productDetail.QuantityRequiredForSpecialPrice)
            {
                // scenario 1 - RQ = 2, items added = 2
                // scenario 2 - RQ = 2, items added = 4
                
                if (_cart[item] % productDetail.QuantityRequiredForSpecialPrice == 0)
                {
                    var timesDiscountWillBeApplied = currentQuantity / productDetail.QuantityRequiredForSpecialPrice;
                    _total -= currentQuantity * productDetail.RegularPrice;
                    _total += productDetail.SpecialPrice * timesDiscountWillBeApplied;
                }
                else
                {
                    var timesDiscountWillBeApplied = currentQuantity / productDetail.QuantityRequiredForSpecialPrice; // 2
                    var quantityBeingDiscounted = timesDiscountWillBeApplied * productDetail.QuantityRequiredForSpecialPrice;
                    _total -= quantityBeingDiscounted * productDetail.RegularPrice;
                    _total += productDetail.SpecialPrice * timesDiscountWillBeApplied;
                }
            }
        }
        
        return _total;
    }

    private void ApplySpecialPrice(Product product)
    {
        _total -= product.RegularPrice * (_cart[product.Sku] - 1);
        _total += product.SpecialPrice;
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