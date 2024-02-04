using System.ComponentModel.DataAnnotations;

namespace CheckoutKata;

public class Product
{
    [Key]
    public string Sku { get; set; }
    
    public int RegularPrice { get; set; }
    
    public int SpecialPrice { get; set; }
    
    public int QuantityRequiredForSpecialPrice { get; set; }
}