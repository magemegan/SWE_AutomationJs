using System.Collections.Generic;
using System.Linq;
using Dapper;
using SWE_AutomationJs_UI_Design.Security;

namespace SWE_AutomationJs_UI_Design.Data
{
    internal static class AppBootstrap
    {
        public static void Initialize()
        {
            EnsureRoles();
            EnsureSeedCredentials();
            EnsureDiningTables();
            EnsureMinimumStaffing();
            EnsureMenuExamples();
        }

        private static void EnsureRoles()
        {
            using (var connection = Db.Open())
            {
                connection.Execute(@"
INSERT OR IGNORE INTO Roles (RoleName) VALUES
    ('Admin'),
    ('Manager'),
    ('Server'),
    ('Cashier'),
    ('Kitchen'),
    ('Busboy');");
            }
        }

        private static void EnsureSeedCredentials()
        {
            var defaultPasswords = new Dictionary<string, string>
            {
                { "E00001", "Admin@123" },
                { "E00002", "Manager@123" },
                { "E00003", "Server@123" },
                { "E00004", "Cashier@123" },
                { "E00005", "Kitchen@123" },
                { "E00010", "Busboy@123" }
            };

            using (var connection = Db.Open())
            {
                foreach (KeyValuePair<string, string> pair in defaultPasswords)
                {
                    string currentHash = connection.ExecuteScalar<string>(
                        "SELECT PasswordHash FROM Employees WHERE EmployeeId = @EmployeeId;",
                        new { EmployeeId = pair.Key });

                    if (!PasswordHasher.IsLegacyPlaceholder(currentHash))
                    {
                        continue;
                    }

                    connection.Execute(
                        "UPDATE Employees SET PasswordHash = @PasswordHash WHERE EmployeeId = @EmployeeId;",
                        new
                        {
                            EmployeeId = pair.Key,
                            PasswordHash = PasswordHasher.HashPassword(pair.Value)
                        });
                }
            }
        }

        private static void EnsureDiningTables()
        {
            using (var connection = Db.Open())
            {
                int availableStatusId = connection.ExecuteScalar<int>(
                    "SELECT TableStatusId FROM TableStatus WHERE StatusName = 'Available';");
                List<DiningTableSeed> existingTables = connection.Query<DiningTableSeed>(@"
SELECT
    TableId,
    TableCode,
    ColumnLetter,
    RowNumber
FROM DiningTables;").ToList();
                HashSet<int> existingTableIds = new HashSet<int>(existingTables.Select(x => x.TableId));
                HashSet<string> occupiedCoordinates = new HashSet<string>(
                    existingTables.Select(x => BuildCoordinateKey(x.ColumnLetter, x.RowNumber)));
                Queue<DiningTableSeed> freeLayouts = new Queue<DiningTableSeed>(
                    BuildDefaultLayouts().Where(x => !occupiedCoordinates.Contains(BuildCoordinateKey(x.ColumnLetter, x.RowNumber))));

                for (int tableId = 1; tableId <= 36; tableId++)
                {
                    if (existingTableIds.Contains(tableId))
                    {
                        continue;
                    }

                    DiningTableSeed layout = freeLayouts.Count > 0
                        ? freeLayouts.Dequeue()
                        : BuildFallbackLayout(tableId);

                    connection.Execute(@"
INSERT INTO DiningTables
    (TableId, TableCode, ColumnLetter, RowNumber, CurrentTableStatusId, SeatCapacity)
VALUES
    (@TableId, @TableCode, @ColumnLetter, @RowNumber, @CurrentTableStatusId, @SeatCapacity);",
                        new
                        {
                            TableId = tableId,
                            TableCode = layout.TableCode,
                            ColumnLetter = layout.ColumnLetter,
                            RowNumber = layout.RowNumber,
                            CurrentTableStatusId = availableStatusId,
                            SeatCapacity = 4
                        });
                }
            }
        }

        private static void EnsureMinimumStaffing()
        {
            using (var connection = Db.Open())
            {
                EnsureEmployee(connection, "E00006", "Elijah", "Carter", "Server", "Server@123");
                EnsureEmployee(connection, "E00007", "Sophia", "Nguyen", "Server", "Server@123");
                EnsureEmployee(connection, "E00008", "Liam", "Walker", "Kitchen", "Kitchen@123");
                EnsureEmployee(connection, "E00009", "Olivia", "Hall", "Kitchen", "Kitchen@123");
                EnsureEmployee(connection, "E00010", "Marcus", "Reed", "Busboy", "Busboy@123");

                EnsureWaiterAssignment(connection, "E00003", 1);
                EnsureWaiterAssignment(connection, "E00003", 2);
                EnsureWaiterAssignment(connection, "E00003", 3);
                EnsureWaiterAssignment(connection, "E00006", 4);
                EnsureWaiterAssignment(connection, "E00006", 5);
                EnsureWaiterAssignment(connection, "E00006", 6);
                EnsureWaiterAssignment(connection, "E00007", 7);
                EnsureWaiterAssignment(connection, "E00007", 8);
                EnsureWaiterAssignment(connection, "E00007", 9);
            }
        }

        private static void EnsureEmployee(Microsoft.Data.Sqlite.SqliteConnection connection, string employeeId, string firstName, string lastName, string roleName, string password)
        {
            long existing = connection.ExecuteScalar<long>(
                "SELECT COUNT(*) FROM Employees WHERE EmployeeId = @EmployeeId;",
                new { EmployeeId = employeeId });

            if (existing > 0)
            {
                return;
            }

            connection.Execute(@"
INSERT INTO Employees (EmployeeId, RoleId, FirstName, LastName, PasswordHash, IsActive)
VALUES (
    @EmployeeId,
    (SELECT RoleId FROM Roles WHERE RoleName = @RoleName),
    @FirstName,
    @LastName,
    @PasswordHash,
    1
);",
                new
                {
                    EmployeeId = employeeId,
                    RoleName = roleName,
                    FirstName = firstName,
                    LastName = lastName,
                    PasswordHash = PasswordHasher.HashPassword(password)
                });
        }

        private static void EnsureWaiterAssignment(Microsoft.Data.Sqlite.SqliteConnection connection, string employeeId, int tableId)
        {
            long employeeExists = connection.ExecuteScalar<long>(
                "SELECT COUNT(*) FROM Employees WHERE EmployeeId = @EmployeeId;",
                new { EmployeeId = employeeId });
            long tableExists = connection.ExecuteScalar<long>(
                "SELECT COUNT(*) FROM DiningTables WHERE TableId = @TableId;",
                new { TableId = tableId });

            if (employeeExists == 0 || tableExists == 0)
            {
                return;
            }

            long existing = connection.ExecuteScalar<long>(
                "SELECT COUNT(*) FROM WaiterTableAssignments WHERE EmployeeId = @EmployeeId AND TableId = @TableId AND UnassignedAt IS NULL;",
                new
                {
                    EmployeeId = employeeId,
                    TableId = tableId
                });

            if (existing > 0)
            {
                return;
            }

            connection.Execute(
                "INSERT INTO WaiterTableAssignments (EmployeeId, TableId) VALUES (@EmployeeId, @TableId);",
                new
                {
                    EmployeeId = employeeId,
                    TableId = tableId
                });
        }

        private static IEnumerable<DiningTableSeed> BuildDefaultLayouts()
        {
            for (int tableId = 1; tableId <= 36; tableId++)
            {
                int zeroBasedIndex = tableId - 1;
                string columnLetter = ((char)('A' + (zeroBasedIndex / 6))).ToString();
                int rowNumber = (zeroBasedIndex % 6) + 1;

                yield return new DiningTableSeed
                {
                    TableId = tableId,
                    TableCode = string.Format("{0}{1}", columnLetter, rowNumber),
                    ColumnLetter = columnLetter,
                    RowNumber = rowNumber
                };
            }
        }

        private static DiningTableSeed BuildFallbackLayout(int tableId)
        {
            int zeroBasedIndex = tableId - 1;
            string columnLetter = ((char)('A' + (zeroBasedIndex / 6))).ToString();
            int rowNumber = (zeroBasedIndex % 6) + 1;

            return new DiningTableSeed
            {
                TableId = tableId,
                TableCode = string.Format("T{0}", tableId),
                ColumnLetter = columnLetter,
                RowNumber = rowNumber
            };
        }

        private static string BuildCoordinateKey(string columnLetter, int rowNumber)
        {
            return string.Format("{0}:{1}", columnLetter, rowNumber);
        }

        private sealed class DiningTableSeed
        {
            public int TableId { get; set; }
            public string TableCode { get; set; }
            public string ColumnLetter { get; set; }
            public int RowNumber { get; set; }
        }

        private static void EnsureMenuExamples()
        {
            using (var connection = Db.Open())
            {
                connection.Execute("INSERT OR IGNORE INTO MenuCategories (CategoryName) VALUES ('Soups/Salads');");
                connection.Execute(@"
INSERT OR IGNORE INTO MenuItems (CategoryId, ItemName, Price, IsActive)
VALUES ((SELECT CategoryId FROM MenuCategories WHERE CategoryName = 'Soups/Salads'), 'Caesar Salad', 9.99, 1);");
            }
        }
    }
}
