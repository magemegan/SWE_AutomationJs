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
        public string RequestType { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public DateTime ShiftDate { get; set; }
    }
}
