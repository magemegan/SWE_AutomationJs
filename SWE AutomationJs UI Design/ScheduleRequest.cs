using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_AutomationJs_UI_Design
{
    internal class ScheduleRequest
    {
        public string EmployeeName { get; set; }
        public string RequestType { get; set; } // "TimeOff" or "Swap"
        public DateTime Date { get; set; } // For TimeOff
        public string Reason { get; set; } // reason for the request
        public string Status { get; set; } // "Pending", "Approved", "Denied"
        public ScheduleRequest() { }
    }
}
