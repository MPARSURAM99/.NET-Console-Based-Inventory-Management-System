using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_Management_System.Utility;

namespace Inventory_Management_System.Menu
{
    public abstract class BaseMenu
    {
        protected Dictionary<string, Action> menuAction;
        protected string menuTitle;
        protected abstract void InitializeMenu();

        public BaseMenu(string heading)
        {
            menuTitle = heading;
            menuAction = new Dictionary<string, Action>();
            InitializeMenu();
        }

        protected void DisplayHeader()
        {
            Console.WriteLine("=====================================");
            Console.WriteLine("            " + menuTitle);
            Console.WriteLine("=====================================");
        }

        protected virtual void DisplayMenuOptions()
        {
            foreach (var option in menuAction)
            {
                string readableOPtion = Utilities.ToReadableSentence(option.Value.Method.Name);
                Console.WriteLine($"{option.Key}. {readableOPtion}");
            }
            Console.WriteLine("0. Go Back");
        }

        public void ShowMenu()
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                DisplayHeader();
                DisplayMenuOptions();
                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                if (choice == "0")
                {
                    isRunning = false;
                }
                else if (menuAction.ContainsKey(choice))
                {
                    menuAction[choice].Invoke();
                }
                else
                {
                    Utilities.CustomMessage("Invalid option. Please try again.", ConsoleColor.Red);
                    Console.ReadKey();
                }
            }
        }
    }
}
