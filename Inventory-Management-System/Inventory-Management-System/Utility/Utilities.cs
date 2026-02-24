using Inventory_Management_System.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory_Management_System.Utility
{
    public class Utilities
    {
        private static string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
        private static string dataFolderPath = Path.Combine(projectDirectory, "Data");

        public static string File_Path = Path.Combine(dataFolderPath, "inventory.json");
        public static void CustomMessage(string message, ConsoleColor color)
        {
            try
            {
                ConsoleColor forGroundColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Thread.Sleep(2000);
                Console.ForegroundColor = forGroundColor;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid color provided: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while printing message: {ex.Message}");
            }
        }

        public static void CheckAndCreateJsonFile()
        {
            try
            {
                Directory.CreateDirectory(dataFolderPath);

                if (!File.Exists(File_Path))
                {
                    File.WriteAllText(File_Path, "[]");
                    CustomMessage("JSON file created successfully.", ConsoleColor.Green);
                }
                else
                {
                    string jsonData = File.ReadAllText(File_Path);
                    var items = JsonConvert.DeserializeObject<List<Inventory>>(jsonData) ?? new List<Inventory>();

                    string updatedJsonData = JsonConvert.SerializeObject(items, Formatting.Indented);
                    File.WriteAllText(File_Path, updatedJsonData);

                    CustomMessage($"JSON file already exists.", ConsoleColor.Yellow);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Error: Access denied to create or write the file.");
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error: IO issue occurred while handling the file.");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error occurred:");
                Console.WriteLine(ex.Message);
            }
        }

        public static string ToReadableSentence(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            return Regex.Replace(text, "(?<!^)([A-Z])", " $1");
        }

        public static object ConvertToPropertyType(string input, Type propertyType)
        {
            try
            {
                if (propertyType == typeof(int))
                    return int.Parse(input);
                else if (propertyType == typeof(decimal))
                    return decimal.Parse(input);
                else if (propertyType == typeof(bool))
                    return bool.Parse(input);
                else
                    return input;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Input format is incorrect: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while converting input: {ex.Message}");
                throw;
            }
        }

        public static T GetUserInput<T>() where T : new()
        {
            T obj = new T();
            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                if (!prop.CanWrite)
                {
                    continue;
                }

                bool hasSkip = Attribute.IsDefined(prop, typeof(SkipAttribute));
                if (hasSkip)
                {
                    continue;
                }

                bool hasRequired = Attribute.IsDefined(prop, typeof(RequiredAttribute));
                while (true)
                {
                    Console.Write($"Enter {ToReadableSentence(prop.Name)}: ");
                    string input = Console.ReadLine();

                    if (hasRequired && string.IsNullOrWhiteSpace(input))
                    {
                        CustomMessage($"{ToReadableSentence(prop.Name)} is required. Please enter a value.", ConsoleColor.Red);
                        continue;
                    }

                    try
                    {
                        object value = ConvertToPropertyType(input, prop.PropertyType);
                        prop.SetValue(obj, value);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Invalid input for {prop.Name}: {ex.Message}");
                        Console.WriteLine("Please try again.");
                    }
                }
            }
            return obj;
        }
    }
}
