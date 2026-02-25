using Inventory_Management_System.Model;
using Inventory_Management_System.Service;
using Inventory_Management_System.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System.Menu
{
    public class MainMenu : BaseMenu
    {
        public MainMenu() : base("Main Menu")
        {
        }

        protected override void InitializeMenu()
        {
            menuAction.Add("1", AddProduct);
            menuAction.Add("2", UpdateProduct);
            menuAction.Add("3", DeliverProduct);
            menuAction.Add("4", ReceiveProduct);
            menuAction.Add("5", ViewInventory);
            menuAction.Add("6", ViewInventoryToReOrder);
        }

        private void AddProduct()
        {
            Inventory newProduct = Utilities.GetUserInput<Inventory>();

            try
            {
                if (InventoryServices.AddInventory(newProduct))
                {
                    Utilities.CustomMessage("Product added successfully!", ConsoleColor.Green);
                }
                else
                {
                    Utilities.CustomMessage("Failed to add product. Please try again.", ConsoleColor.Red);

                }
            }
            catch (Exception ex)
            {
                Utilities.CustomMessage($"An error occurred: {ex.Message}", ConsoleColor.Red);
            }
        }

        private void UpdateProduct()
        {
            Console.WriteLine("Search the Product name to update: ");
            string keyword = Console.ReadLine();

            if (keyword != null)
            {
                var searchResults = InventoryServices.SearchInventory(keyword);
                if (searchResults.Count > 0)
                {
                    Console.WriteLine("Search Results:");
                    foreach (var item in searchResults)
                    {
                        Console.WriteLine($"ID: {item.Id}, Name: {item.ProductName}, Category: {item.ProductCategory}");
                    }
                    Console.WriteLine("Enter the name of the product you want to update:");
                    if (!int.TryParse(Console.ReadLine(), out int productId))
                    {
                        Utilities.CustomMessage("Invalid ID format. Please enter a valid integer ID.", ConsoleColor.Red);
                        return;
                    }

                    Inventory updatedProduct = Utilities.GetUserInput<Inventory>();
                    try
                    {
                        if (InventoryServices.UpdateInventory(productId, updatedProduct))
                        {
                            Utilities.CustomMessage("Product updated successfully!", ConsoleColor.Green);
                        }
                        else
                        {
                            Utilities.CustomMessage("Failed to update product. Please try again.", ConsoleColor.Red);
                        }
                    }
                    catch (Exception ex)
                    {
                        Utilities.CustomMessage($"An error occurred: {ex.Message}", ConsoleColor.Red);
                    }
                }
                else
                {
                    Utilities.CustomMessage("No products found matching the search criteria.", ConsoleColor.Yellow);
                }
            }
            else
            {
                Utilities.CustomMessage("Search keyword cannot be empty. Please enter a valid keyword.", ConsoleColor.Red);
            }
        }

        private void DeliverProduct()
        {
            Console.WriteLine("Search the Product name to Deliver: ");
            string keyword = Console.ReadLine();

            if (keyword != null)
            {
                var searchResults = InventoryServices.SearchInventory(keyword);
                if (searchResults.Count > 0)
                {
                    Console.WriteLine("Search Results:");
                    foreach (var item in searchResults)
                    {
                        Console.WriteLine($"ID: {item.Id}, Name: {item.ProductName}, Category: {item.ProductCategory}");
                    }

                    Console.WriteLine("Enter the ID of the product you want to deliver:");
                    if (!int.TryParse(Console.ReadLine(), out int productId))
                    {
                        Utilities.CustomMessage("Invalid ID format. Please enter a valid integer ID.", ConsoleColor.Red);
                        return;
                    }

                    Console.WriteLine("Enter the quantity to deliver:");
                    if (!int.TryParse(Console.ReadLine(), out int quantity))
                    {
                        Utilities.CustomMessage("Invalid quantity format. Please enter a valid integer quantity.", ConsoleColor.Red);
                        return;
                    }

                    InventoryServices.LowStockAlert += (item) =>
                    {
                        Utilities.CustomMessage($"Low Stock Alert: {item.ProductName} has only {item.StockQuantity} left (Reorder Level: {item.ReorderLevel})", ConsoleColor.Yellow);
                    };

                    try
                    {
                        if (InventoryServices.DeliverInventory(productId, quantity))
                        {
                            Utilities.CustomMessage("Product delivered successfully!", ConsoleColor.Green);
                        }
                        else
                        {
                            Utilities.CustomMessage("Failed to deliver product. Please try again.", ConsoleColor.Red);
                        }
                    }
                    catch (Exception ex)
                    {
                        Utilities.CustomMessage($"An error occurred: {ex.Message}", ConsoleColor.Red);
                    }
                }
                else
                {
                    Utilities.CustomMessage("No products found matching the search criteria.", ConsoleColor.Yellow);
                }
            }
            else
            {
                Utilities.CustomMessage("Search keyword cannot be empty. Please enter a valid keyword.", ConsoleColor.Red);
            }
        }

        private void ReceiveProduct()
        {
            Console.WriteLine("Search the Product name to Receive: ");
            string keyword = Console.ReadLine();

            if (keyword != null)
            {
                var searchResults = InventoryServices.SearchInventory(keyword);
                if (searchResults.Count > 0)
                {
                    Console.WriteLine("Search Results:");
                    foreach (var item in searchResults)
                    {
                        Console.WriteLine($"ID: {item.Id}, Name: {item.ProductName}, Category: {item.ProductCategory}");
                    }
                    Console.WriteLine("Enter the ID of the product you want to receive:");
                    if (!int.TryParse(Console.ReadLine(), out int productId))
                    {
                        Utilities.CustomMessage("Invalid ID format. Please enter a valid integer ID.", ConsoleColor.Red);
                        return;
                    }
                    Console.WriteLine("Enter the quantity to receive:");
                    if (!int.TryParse(Console.ReadLine(), out int quantity))
                    {
                        Utilities.CustomMessage("Invalid quantity format. Please enter a valid integer quantity.", ConsoleColor.Red);
                        return;
                    }
                    try
                    {
                        if (InventoryServices.ReceiveInventory(productId, quantity))
                        {
                            Utilities.CustomMessage("Product received successfully!", ConsoleColor.Green);
                        }
                        else
                        {
                            Utilities.CustomMessage("Failed to receive product. Please try again.", ConsoleColor.Red);
                        }
                    }
                    catch (Exception ex)
                    {
                        Utilities.CustomMessage($"An error occurred: {ex.Message}", ConsoleColor.Red);
                    }
                }
                else
                {
                    Utilities.CustomMessage("No products found matching the search criteria.", ConsoleColor.Yellow);
                }
            }
            else
            {
                Utilities.CustomMessage("Search keyword cannot be empty. Please enter a valid keyword.", ConsoleColor.Red);
            }
        }

        private void ViewInventory()
        {
            var inventoryList = InventoryServices.GetAllInventory();
            if (inventoryList.Count > 0)
            {
                Console.WriteLine("Current Inventory:");
                foreach (var item in inventoryList)
                {
                    Console.WriteLine($"ID: {item.Id}, Name: {item.ProductName}, Category: {item.ProductCategory}, Stock: {item.StockQuantity}, Reorder Level: {item.ReorderLevel}");
                }
            }
            else
            {
                Utilities.CustomMessage("No products in inventory.", ConsoleColor.Yellow);
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ViewInventoryToReOrder()
        {
            var lowStockItems = InventoryServices.GetInventoryToReOrder();
            if (lowStockItems.Count > 0)
            {
                Console.WriteLine("Products that need to be reordered:");
                foreach (var item in lowStockItems)
                {
                    Console.WriteLine($"ID: {item.Id}, Name: {item.ProductName}, Category: {item.ProductCategory}, Stock: {item.StockQuantity}, Reorder Level: {item.ReorderLevel}");
                }
            }
            else
            {
                Utilities.CustomMessage("No products need to be reordered.", ConsoleColor.Green);
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
