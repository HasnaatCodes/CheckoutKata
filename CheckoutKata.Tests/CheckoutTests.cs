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
    public void SingleItem_ShouldReturnCorrectTotal()
    {
        // Arrange
        ICheckout checkout = new Checkout(new ProductDatabaseContext());
        
        // Act
        checkout.Scan("A");
        var actual = checkout.GetTotalPrice();
        
        // Assert
        Assert.That(actual, Is.EqualTo(50));
    }
    
    [Test]
    public void MultipleItems_SameType_ShouldReturnCorrectTotal()
    {
        // Arrange
        ICheckout checkout = new Checkout(new ProductDatabaseContext());
        
        // Act
        checkout.Scan("A");
        checkout.Scan("A");
        var actual = checkout.GetTotalPrice();
        
        // Assert
        Assert.That(actual, Is.EqualTo(100));
    }
    
    [Test]
    public void MultipleItems_DifferentTypes_ShouldReturnCorrectTotal()
    {
        // Arrange
        ICheckout checkout = new Checkout(new ProductDatabaseContext());
        
        // Act
        checkout.Scan("A");
        checkout.Scan("B");
        checkout.Scan("A");
        checkout.Scan("C");
        var actual = checkout.GetTotalPrice();
        
        // Assert
        Assert.That(actual, Is.EqualTo(150));
    }
    
    [Test]
    public void MultipleItemsWithSpecialOffer_SameType_ShouldReturnCorrectTotal()
    {
        // Arrange
        ICheckout checkout = new Checkout(new ProductDatabaseContext());
        
        // Act
        checkout.Scan("B");
        checkout.Scan("B");
        var actual = checkout.GetTotalPrice();
        
        // Assert
        Assert.That(actual, Is.EqualTo(45));
    }
    
    [Test]
    public void MultipleItemsWithSpecialOffer_DifferentTypes_ShouldReturnCorrectTotal()
    {
        // Arrange
        ICheckout checkout = new Checkout(new ProductDatabaseContext());
        
        // Act
        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("B");
        checkout.Scan("B");
        var actual = checkout.GetTotalPrice();
        
        // Assert
        Assert.That(actual, Is.EqualTo(175));
    }
    
    [Test]
    public void AddMultipleItemsWithSpecialOfferTwice_OfSameType_ShouldReturnCorrectTotal()
    {
        // Arrange
        ICheckout checkout = new Checkout(new ProductDatabaseContext());
        
        // Act
        checkout.Scan("B");
        checkout.Scan("B");
        checkout.Scan("B");
        checkout.Scan("B");
        var actual = checkout.GetTotalPrice();
        
        // Assert
        Assert.That(actual, Is.EqualTo(90));
    }
    
    [Test]
    public void AddOddMultipleItemsWithSpecialOfferTwice_OfSameType_ShouldReturnCorrectTotal()
    {
        // Arrange
        ICheckout checkout = new Checkout(new ProductDatabaseContext());
        
        // Act
        checkout.Scan("B");
        checkout.Scan("B");
        checkout.Scan("B");
        checkout.Scan("B");
        checkout.Scan("B");
        var actual = checkout.GetTotalPrice();
        
        // Assert
        Assert.That(actual, Is.EqualTo(120));
    }
    
    [Test]
    public void AddMultipleItemsWithSpecialOfferTwice_OfDifferentType_ShouldReturnCorrectTotal()
    {
        // Arrange
        ICheckout checkout = new Checkout(new ProductDatabaseContext());
        
        // Act
        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("A");
        
        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("A");
        
        
        checkout.Scan("B");
        checkout.Scan("B");
        
        checkout.Scan("B");
        checkout.Scan("B");
        
        var actual = checkout.GetTotalPrice();
        
        // Assert
        Assert.That(actual, Is.EqualTo(350));
    }
    
    [Test]
    public void AddOddMultipleItemsWithSpecialOfferTwice_OfDifferentType_ShouldReturnCorrectTotal()
    {
        // Arrange
        ICheckout checkout = new Checkout(new ProductDatabaseContext());
        
        // Act
        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("A");
        
        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("A");
        
        checkout.Scan("A");
        
        checkout.Scan("B");
        checkout.Scan("B");
        
        checkout.Scan("B");
        checkout.Scan("B");
        
        checkout.Scan("B");
        
        var actual = checkout.GetTotalPrice();
        
        // Assert
        Assert.That(actual, Is.EqualTo(430));
    }
    
    [Test]
    public void EmptyCart_ShouldReturnZero()
    {
        // Arrange
        ICheckout checkout = new Checkout(new ProductDatabaseContext());
        
        // Act
        var actual = checkout.GetTotalPrice();
        
        // Assert
        Assert.That(actual, Is.EqualTo(0));
    }
    
    [Test]
    public void ScanInvalidItem_ShouldReturnZero()
    {
        // Arrange
        ICheckout checkout = new Checkout(new ProductDatabaseContext());
        
        // Act
        checkout.Scan("Z");
        var actual = checkout.GetTotalPrice();
        
        // Assert
        Assert.That(actual, Is.EqualTo(0));
    }
}