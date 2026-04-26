using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Security;

namespace SWE_AutomationJs_UI_Design
{
    internal class EmployeeStorage
    {
        static EmployeeStorage()
        {
            Refresh();
        }

        public static List<Employee> Employees { get; private set; }

        public static void Refresh()
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT
    e.EmployeeId,
    e.FirstName || ' ' || e.LastName AS Name,
    CASE WHEN r.RoleName = 'Server' THEN 'Waiter' ELSE r.RoleName END AS Role,
    '' AS Phone,
    lower(e.FirstName || '.' || e.LastName || '@automationjs.local') AS Email,
    'Full-Time' AS EmploymentType
FROM Employees e
INNER JOIN Roles r ON r.RoleId = e.RoleId
WHERE e.IsActive = 1
ORDER BY e.EmployeeId;";

                Employees = connection.Query<Employee>(sql).ToList();

                foreach (Employee employee in Employees)
                {
                    employee.AssignedTables = connection.Query<int>(@"
SELECT TableId
FROM WaiterTableAssignments
WHERE EmployeeId = @EmployeeId
  AND UnassignedAt IS NULL
ORDER BY TableId;",
                        new { employee.EmployeeId }).ToList();
                }
            }
        }

        public static void Save()
        {
            using (var connection = Db.Open())
            using (var transaction = connection.BeginTransaction())
            {
                foreach (Employee employee in Employees)
                {
                    string firstName;
                    string lastName;
                    SplitName(employee.Name, out firstName, out lastName);
                    string roleName = ToDatabaseRole(employee.Role);

                    connection.Execute(@"
INSERT OR IGNORE INTO Roles (RoleName) VALUES (@RoleName);",
                        new { RoleName = roleName }, transaction);

                    int roleId = connection.ExecuteScalar<int>(@"
SELECT RoleId FROM Roles WHERE RoleName = @RoleName;",
                        new { RoleName = roleName }, transaction);

                    int exists = connection.ExecuteScalar<int>(@"
SELECT COUNT(*) FROM Employees WHERE EmployeeId = @EmployeeId;",
                        new { employee.EmployeeId }, transaction);

                    if (exists == 0)
                    {
                        connection.Execute(@"
INSERT INTO Employees (EmployeeId, RoleId, FirstName, LastName, PasswordHash, IsActive)
VALUES (@EmployeeId, @RoleId, @FirstName, @LastName, @PasswordHash, 1);",
                            new
                            {
                                employee.EmployeeId,
                                RoleId = roleId,
                                FirstName = firstName,
                                LastName = lastName,
                                PasswordHash = PasswordHasher.HashPassword(DefaultPasswordForRole(roleName))
                            }, transaction);
                    }
                    else
                    {
                        connection.Execute(@"
UPDATE Employees
SET RoleId = @RoleId,
    FirstName = @FirstName,
    LastName = @LastName,
    IsActive = 1
WHERE EmployeeId = @EmployeeId;",
                            new
                            {
                                employee.EmployeeId,
                                RoleId = roleId,
                                FirstName = firstName,
                                LastName = lastName
                            }, transaction);
                    }

                    connection.Execute(@"
UPDATE WaiterTableAssignments
SET UnassignedAt = CURRENT_TIMESTAMP
WHERE EmployeeId = @EmployeeId
  AND UnassignedAt IS NULL;",
                        new { employee.EmployeeId }, transaction);

                    if (roleName == "Server" && employee.AssignedTables != null)
                    {
                        foreach (int tableId in employee.AssignedTables.Distinct())
                        {
                            int tableExists = connection.ExecuteScalar<int>(@"
SELECT COUNT(*) FROM DiningTables WHERE TableId = @TableId;",
                                new { TableId = tableId }, transaction);
                            if (tableExists == 0)
                            {
                                continue;
                            }

                            connection.Execute(@"
INSERT INTO WaiterTableAssignments (EmployeeId, TableId)
VALUES (@EmployeeId, @TableId);",
                                new
                                {
                                    employee.EmployeeId,
                                    TableId = tableId
                                }, transaction);
                        }
                    }
                }

                transaction.Commit();
            }

            Refresh();
        }

        public static void DeleteAt(int index)
        {
            if (index < 0 || index >= Employees.Count)
            {
                return;
            }

            string employeeId = Employees[index].EmployeeId;
            using (var connection = Db.Open())
            {
                connection.Execute(@"
UPDATE Employees
SET IsActive = 0
WHERE EmployeeId = @EmployeeId;",
                    new { EmployeeId = employeeId });

                connection.Execute(@"
UPDATE WaiterTableAssignments
SET UnassignedAt = CURRENT_TIMESTAMP
WHERE EmployeeId = @EmployeeId
  AND UnassignedAt IS NULL;",
                    new { EmployeeId = employeeId });
            }

            Refresh();
        }

        private static string ToDatabaseRole(string uiRole)
        {
            if (string.Equals(uiRole, "Waiter", StringComparison.OrdinalIgnoreCase))
            {
                return "Server";
            }

            return string.IsNullOrWhiteSpace(uiRole) ? "Server" : uiRole.Trim();
        }

        private static string DefaultPasswordForRole(string roleName)
        {
            switch (roleName)
            {
                case "Admin":
                    return "Admin@123";
                case "Manager":
                    return "Manager@123";
                case "Kitchen":
                    return "Kitchen@123";
                case "Busboy":
                    return "Busboy@123";
                case "Server":
                    return "Server@123";
                default:
                    return "Welcome@123";
            }
        }

        private static void SplitName(string name, out string firstName, out string lastName)
        {
            string cleaned = string.IsNullOrWhiteSpace(name) ? "New Employee" : name.Trim();
            string[] parts = cleaned.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            firstName = parts.Length > 0 ? parts[0] : "New";
            lastName = parts.Length > 1 ? parts[1] : "Employee";
        }
    }
}
