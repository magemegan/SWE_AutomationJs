using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SWE_AutomationJs_UI_Design
{
    public class Employee
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string EmploymentType { get; set; }
        [XmlArray("AssignedTables")]
        [XmlArrayItem("TableId")]
        public List<int> AssignedTables { get; set; } = new List<int>();
    }
}
