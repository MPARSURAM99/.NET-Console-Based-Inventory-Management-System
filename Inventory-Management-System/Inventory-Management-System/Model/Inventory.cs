using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System.Model
{
    public class Inventory
    {
        [SkipAttribute]
        public int Id { get; set; }

        [RequiredAttribute]
        public string ProductName { get; set; }

        [RequiredAttribute]
        public string ProductCategory { get; set; }

        [RequiredAttribute]
        public int StockQuantity { get; set; }

        [RequiredAttribute]
        public int ReorderLevel { get; set; }
    }
}
