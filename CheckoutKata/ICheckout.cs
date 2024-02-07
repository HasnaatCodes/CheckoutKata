namespace CheckoutKata;

public interface ICheckout
{
    public void Scan(string item);
    
    public decimal GetTotalPrice();
    
    public decimal GetTotalSavings();
}