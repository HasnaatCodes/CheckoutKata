using CheckoutKata.DatabaseContext;
using CheckoutKata.Exceptions;

namespace CheckoutKata.Tests;

public class CheckoutTests
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
        var actualTotalSavings = checkout.GetTotalSavings();
        
        // Assert
        Assert.That(actual, Is.EqualTo(175));
        Assert.That(actualTotalSavings, Is.EqualTo(35));
        
    }
    
    [Test]
    public void AddMultipleItemsWithSpecialOfferMultipleTimes_OfSameType_ShouldReturnCorrectTotal()
    {
        // Arrange
        ICheckout checkout = new Checkout(new ProductDatabaseContext());
        
        // Act
        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("A");
        var actual = checkout.GetTotalPrice();
        
        // Assert
        Assert.That(actual, Is.EqualTo(230));
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
        try
        {
            checkout.Scan("Z");
        }
        catch (NotFoundException)
        {
            
        }
        var actual = checkout.GetTotalPrice();
        
        // Assert
        Assert.That(actual, Is.EqualTo(0));
    }
}