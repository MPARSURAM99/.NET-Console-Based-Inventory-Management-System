using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_Management_System.Menu;
using Inventory_Management_System.Service;
using Inventory_Management_System.Utility;

namespace Inventory_Management_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Inventory Management System";
            Console.ForegroundColor = ConsoleColor.White;

            Utilities.CheckAndCreateJsonFile();

            Console.WriteLine("Wellcome to the Inventory Management System!");

            MainMenu mainMenu = new MainMenu();
            mainMenu.ShowMenu();
        }
    }
}
