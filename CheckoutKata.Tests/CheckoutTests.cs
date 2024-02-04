using CheckoutKata;
using CheckoutKata.DatabaseContext;
using Moq;

namespace TestProject1;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Checkout_ScanItem_ShouldReturnCorrectTotal()
    {
        ICheckout checkout = new Checkout(new ProductDatabaseContext());
        
        checkout.Scan("A");
        checkout.Scan("B");
        checkout.Scan("A");
        
        var actual = checkout.GetTotalPrice();
        Assert.That(actual, Is.EqualTo(130));
    }
}