using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_AutomationJs_UI_Design
{
    public class Order
    {
        public int TableNumber { get; set; }
        public List<string> Items { get; set; }
        public string Status { get; set; }

        public Order()
        {
            Items = new List<string>();
            Status = "Pending";
        }

        public Order(int tableNumber, List<string> items)
        {
            TableNumber = tableNumber;
            Items = items;
            Status = "Pending";
        }
    }
}
