namespace CheckoutKata;

public class Checkout : ICheckout
{
    private int total = 0;

    private Dictionary<string, int> value = new Dictionary<string, int>
    {
        { "A", 10 },
        { "B", 5 }
    };
    
    public void Scan(string item)
    {
        total += value[item];
    }

    public int GetTotalPrice()
    {
        return total;
    }
}