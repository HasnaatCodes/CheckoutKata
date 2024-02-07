// See https://aka.ms/new-console-template for more information

using CheckoutKata;
using CheckoutKata.DatabaseContext;
using CheckoutKata.Exceptions;

var dbContext = new ProductDatabaseContext();
var checkout = new Checkout(dbContext);

while (true)
{
    Console.WriteLine("Please enter the SKU of the item you want to scan or type 'exit' to quit:");
    string input = Console.ReadLine().Trim();

    if (input == "exit")
    {
        break;
    }

    try
    {
        checkout.Scan(input);
    }
    catch (NotFoundException exception)
    {
        Console.WriteLine(exception.Message);
    }
}

var totalPrice = checkout.GetTotalPrice();
Console.WriteLine($"Total Price: {totalPrice:C}");

var totalSavings = checkout.GetTotalSavings();
Console.WriteLine($"Total Savings: {totalSavings:C}");

Console.WriteLine("Press any key to exit...");
Console.ReadKey();