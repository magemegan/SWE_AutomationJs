using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace SWE_AutomationJs_UI_Design
{
    internal class EmployeeStorage
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "employee-profiles.xml");
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(List<Employee>));

        static EmployeeStorage()
        {
            Employees = LoadEmployees();
            Save();
        }

        public static List<Employee> Employees { get; private set; }

        public static void Save()
        {
            using (FileStream stream = File.Create(FilePath))
            {
                Serializer.Serialize(stream, Employees);
            }
        }

        private static List<Employee> LoadEmployees()
        {
            if (File.Exists(FilePath))
            {
                using (FileStream stream = File.OpenRead(FilePath))
                {
                    return (List<Employee>)Serializer.Deserialize(stream);
                }
            }

            return new List<Employee>
            {
                new Employee { EmployeeId = "E00001", Name = "Tiffany Brandon", Role = "Manager", Phone = "555-0101", Email = "tiffany@automationjs.local", EmploymentType = "Full-Time" },
                new Employee { EmployeeId = "E00003", Name = "Ava Smith", Role = "Waiter", Phone = "555-0103", Email = "ava@automationjs.local", EmploymentType = "Full-Time", AssignedTables = new List<int> { 1, 2, 3 } },
                new Employee { EmployeeId = "E00006", Name = "Elijah Carter", Role = "Waiter", Phone = "555-0106", Email = "elijah@automationjs.local", EmploymentType = "Full-Time", AssignedTables = new List<int> { 4, 5, 6 } },
                new Employee { EmployeeId = "E00007", Name = "Sophia Nguyen", Role = "Waiter", Phone = "555-0107", Email = "sophia@automationjs.local", EmploymentType = "Part-Time", AssignedTables = new List<int> { 7, 8, 9 } },
                new Employee { EmployeeId = "E00005", Name = "Mia Brown", Role = "Kitchen", Phone = "555-0105", Email = "mia@automationjs.local", EmploymentType = "Full-Time" },
                new Employee { EmployeeId = "E00008", Name = "Liam Walker", Role = "Kitchen", Phone = "555-0108", Email = "liam@automationjs.local", EmploymentType = "Full-Time" },
                new Employee { EmployeeId = "E00009", Name = "Olivia Hall", Role = "Kitchen", Phone = "555-0109", Email = "olivia@automationjs.local", EmploymentType = "Part-Time" }
            };
        }
    }
}
