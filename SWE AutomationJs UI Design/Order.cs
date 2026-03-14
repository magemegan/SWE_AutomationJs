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
    }
}
