using Inventory_Management_System.Model;
using Inventory_Management_System.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System.Service
{
    public static class InventoryServices
    {
        public static bool AddInventory(Inventory inventory)
        {
            try
            {
                string jsonData = File.ReadAllText(Utilities.File_Path);
                var items = JsonConvert.DeserializeObject<List<Inventory>>(jsonData) ?? new List<Inventory>();

                int nextId = items.Any() ? items.Max(i => i.Id) + 1 : 1;
                inventory.Id = nextId;
                items.Add(inventory);

                string updatedJsonData = JsonConvert.SerializeObject(items, Formatting.Indented);
                File.WriteAllText(Utilities.File_Path, updatedJsonData);
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }

        public static List<Inventory> SearchInventory(string keyword)
        {
            try
            {
                string jsonData = File.ReadAllText(Utilities.File_Path);
                var items = JsonConvert.DeserializeObject<List<Inventory>>(jsonData) ?? new List<Inventory>();

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return new List<Inventory>();
                }

                keyword = keyword.Trim();

                return items.Where(i =>
                    (i.Id.ToString().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(i.ProductName) && i.ProductName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(i.ProductCategory) && i.ProductCategory.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return new List<Inventory>();
            }
        }

        public static bool UpdateInventory(int id, Inventory inventory)
        {
            try
            {
                string jsonData = File.ReadAllText(Utilities.File_Path);
                var items = JsonConvert.DeserializeObject<List<Inventory>>(jsonData) ?? new List<Inventory>();
                var itemToUpdate = items.FirstOrDefault(i => i.Id == id);

                if (itemToUpdate != null)
                {
                    itemToUpdate.ProductName = inventory.ProductName;
                    itemToUpdate.ProductCategory = inventory.ProductCategory;
                    itemToUpdate.StockQuantity = inventory.StockQuantity;
                    itemToUpdate.ReorderLevel = inventory.ReorderLevel;
                    string updatedJsonData = JsonConvert.SerializeObject(items, Formatting.Indented);
                    File.WriteAllText(Utilities.File_Path, updatedJsonData);
                }
                return true;

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }

        public static event Action<Inventory> LowStockAlert;


        public static bool DeliverInventory(int id, int quantity)
        {
            try
            {
                string jsonData = File.ReadAllText(Utilities.File_Path);
                var items = JsonConvert.DeserializeObject<List<Inventory>>(jsonData) ?? new List<Inventory>();
                var itemToUpdate = items.FirstOrDefault(i => i.Id == id);
                if (itemToUpdate != null)
                {
                    itemToUpdate.StockQuantity -= quantity;

                    if ((itemToUpdate.StockQuantity - 2) <= itemToUpdate.ReorderLevel)
                    {
                        LowStockAlert?.Invoke(itemToUpdate);
                    }

                    string updatedJsonData = JsonConvert.SerializeObject(items, Formatting.Indented);
                    File.WriteAllText(Utilities.File_Path, updatedJsonData);
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }

        public static bool ReceiveInventory(int id, int quantity)
        {
            try
            {
                string jsonData = File.ReadAllText(Utilities.File_Path);
                var items = JsonConvert.DeserializeObject<List<Inventory>>(jsonData) ?? new List<Inventory>();
                var itemToUpdate = items.FirstOrDefault(i => i.Id == id);
                if (itemToUpdate != null)
                {
                    itemToUpdate.StockQuantity += quantity;

                    if ((itemToUpdate.StockQuantity - 2) <= itemToUpdate.ReorderLevel)
                    {
                        LowStockAlert?.Invoke(itemToUpdate);
                    }

                    string updatedJsonData = JsonConvert.SerializeObject(items, Formatting.Indented);
                    File.WriteAllText(Utilities.File_Path, updatedJsonData);
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }

        public static List<Inventory> GetAllInventory()
        {
            try
            {
                string jsonData = File.ReadAllText(Utilities.File_Path);
                var items = JsonConvert.DeserializeObject<List<Inventory>>(jsonData) ?? new List<Inventory>();
                return items;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return new List<Inventory>();
            }
        }

        public static List<Inventory> GetInventoryToReOrder()
        {
            try
            {
                string jsonData = File.ReadAllText(Utilities.File_Path);
                var items = JsonConvert.DeserializeObject<List<Inventory>>(jsonData) ?? new List<Inventory>();
                return items.Where(i => (i.StockQuantity - 2) <= i.ReorderLevel).ToList();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return new List<Inventory>();
            }
        }
    }
}
