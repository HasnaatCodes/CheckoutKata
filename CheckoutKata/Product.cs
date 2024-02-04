using System.ComponentModel.DataAnnotations;

namespace CheckoutKata;

public class Product
{
    [Key]
    public string Sku { get; set; }
    
    public double RegularPrice { get; set; }
    
    public double SpecialPrice { get; set; }
    
    public int QuantityRequiredForSpecialPrice { get; set; }
}