using System;
using System.Collections.Generic;
using System.Linq;

// Define a base class for items
public class Item
{
    public string Type { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public double Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string Country { get; set; }

    public Item(string type, string brand, string model, double price, DateTime purchaseDate, string country)
    {
        Type = type;
        Brand = brand;
        Model = model;
        Price = price;
        PurchaseDate = purchaseDate;
        Country = country;
    }

    // Function to check if the item is older than a certain number of days
    public bool IsOlderThanDays(int days)
    {
        return (DateTime.Now - PurchaseDate).TotalDays > days;
    }

    // Function to check if the item's age is within a specific range
    public bool IsAgeInRange(int startDays, int endDays)
    {
        int ageInDays = (int)(DateTime.Now - PurchaseDate).TotalDays;
        return ageInDays >= startDays && ageInDays <= endDays;
    }

    // Function to convert USD price to local currency
    public double ConvertToLocalCurrency(double exchangeRate)
    {
        return Price * exchangeRate;
    }
}

// Inherit from the base class for computers
public class Computer : Item
{
    public Computer(string brand, string model, double price, DateTime purchaseDate, string country)
        : base("Computer", brand, model, price, purchaseDate, country)
    {
    }
}

// Inherit from the base class for phones
public class Phone : Item
{
    public Phone(string brand, string model, double price, DateTime purchaseDate, string country)
        : base("Phone", brand, model, price, purchaseDate, country)
    {
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create lists to hold items (both computers and phones)
        List<Item> items = new List<Item>();

        // Hardcode a computer and a phone with a purchase date to be abel to chek age of items
        items.Add(new Computer("HardcodedBrand", "HardcodedModel", 999.99, new DateTime(2019, 1, 1), "USA"));
        items.Add(new Phone("HardcodedBrand", "HardcodedModel", 799.99, new DateTime(2021, 8, 12), "Germany")); // This date gave me the yellow text for age of item

        // Main program loop
        while (true)
        {
            Console.WriteLine("\nPress:");
            Console.WriteLine("  J to add an item to Japan");
            Console.WriteLine("  G to add an item to Germany");
            Console.WriteLine("  U to add an item to USA");
            Console.WriteLine("  P to print the table");
            Console.WriteLine("  Q to exit");

            char choice = Console.ReadKey().KeyChar;

            switch (char.ToUpper(choice))
            {
                case 'J':
                case 'G':
                case 'U':
                    Console.WriteLine("\nEnter the item details:");
                    Console.Write("Type (Computer/Phone): ");
                    string itemType = Console.ReadLine();
                    Console.Write("Brand: ");
                    string brand = Console.ReadLine();
                    Console.Write("Model: ");
                    string model = Console.ReadLine();
                    Console.Write("Price (in USD): ");
                    double price;
                    while (!double.TryParse(Console.ReadLine(), out price))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid price:");
                    }

                    string country = (char.ToUpper(choice) == 'J') ? "Japan" :
                                     (char.ToUpper(choice) == 'G') ? "Germany" :
                                     (char.ToUpper(choice) == 'U') ? "USA" : "";

                    DateTime purchaseDate = DateTime.Now;

                    // Add the new item to the list
                    if (itemType.Equals("Computer", StringComparison.OrdinalIgnoreCase))
                    {
                        items.Add(new Computer(brand, model, price, purchaseDate, country));
                    }
                    else if (itemType.Equals("Phone", StringComparison.OrdinalIgnoreCase))
                    {
                        items.Add(new Phone(brand, model, price, purchaseDate, country));
                    }
                    else
                    {
                        Console.WriteLine("Invalid item type. Please enter either 'Computer' or 'Phone'.");
                    }
                    break;

                case 'P':
                    // Print the table
                    PrintTable(items);
                    break;

                case 'Q':
                    // Exit the program
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("\nInvalid choice. Please try again.");
                    break;
            }
        }
    }

    // Function to print the table
    static void PrintTable(List<Item> items)
    {
        // Sort items first by country, then by purchase date
        items = items.OrderBy(i => i.Country).ThenBy(i => i.PurchaseDate).ToList();

        // Display information about items (computers and phones) in a table format
        Console.WriteLine("\nItems:");
        Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
        Console.WriteLine("| Type     | Brand          | Model         | Country | Purchase Date | Price in USD | Local Price |");
        Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
        foreach (var item in items)
        {
            // Check if the item is older than 1000 days or in the range of 910 to 999 days
            string textColor = item.IsOlderThanDays(1000) ? "Red" :
                               item.IsAgeInRange(910, 999) ? "Yellow" : "White";

            // Get the exchange rate for the country (assuming a simple conversion for demonstration)
            double exchangeRate = GetExchangeRate(item.Country);

            // Convert the USD price to local currency
            double localPrice = item.ConvertToLocalCurrency(exchangeRate);

            // Print the item with the specified text color
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), textColor, true);
            Console.WriteLine($"| {item.Type,-8} | {item.Brand,-15} | {item.Model,-13} | {item.Country,-7} | {item.PurchaseDate.ToString("dd/MM/yyyy"),-14} | {item.Price,-12:F2} | {localPrice,-11:F2} |");
            Console.ResetColor();
        }
        Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
    }

    // Function to get the exchange rate for the country (dummy values for demonstration)
    static double GetExchangeRate(string country)
    {
        // You can replace these dummy values with actual exchange rates
        switch (country.ToUpper())
        {
            case "USA":
                return 1.0; // USD to USD
            case "GERMANY":
                return 0.85; // USD to EUR
            case "JAPAN":
                return 109.5; // USD to JPY
            default:
                return 1.0; // Default to USD
        }
    }
}
