using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_AutomationJs_UI_Design
{
    internal class Request
    {
        // This class represents a request made by a customer at a table. It contains properties for the table number, item name, quantity, priority, notes, and status of the request.
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string Priority { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
    }
}
