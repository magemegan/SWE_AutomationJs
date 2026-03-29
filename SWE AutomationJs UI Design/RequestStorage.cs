using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_AutomationJs_UI_Design
{
    internal class RequestStorage
    {
        // This class is responsible for storing the current requests made by customers.
        // It uses a list to keep track of all active requests in the restaurant.
        public static List<Request> RequestOrder = new List<Request>();
    }
}
