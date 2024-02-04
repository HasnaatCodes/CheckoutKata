using CheckoutKata;
using Moq;

namespace TestProject1;

public class Tests
{
    private Mock<ICheckout> _mockCheckout;
    [SetUp]
    public void Setup()
    {
        _mockCheckout = new Mock<ICheckout>();
    }

    [Test]
    public void Checkout_ScanItem_ShouldReturnCorrectTotal()
    {
        ICheckout checkout = new Checkout();
        
        checkout.Scan("A");
        checkout.Scan("B");
        checkout.Scan("A");
        
        var actual = checkout.GetTotalPrice();
        Assert.That(actual, Is.EqualTo(25));
    }
}