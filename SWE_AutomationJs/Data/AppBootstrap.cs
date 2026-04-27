using System.Collections.Generic;
using System.Linq;
using Dapper;
using SWE_AutomationJs_UI_Design.Security;
using Microsoft.Data.Sqlite;

namespace SWE_AutomationJs_UI_Design.Data
{
    internal static class AppBootstrap
    {
        public static void Initialize()
        {
            EnsureSchemaCompatibility();
            EnsurePaymentViews();
            EnsureRoles();
            EnsureSeedCredentials();
            EnsureDiningTables();
            EnsureMinimumStaffing();
            MenuCatalogBootstrap.EnsureConfiguredMenu();
            EnsureInventoryItems();
        }

        private static void EnsureSchemaCompatibility()
        {
            using (var connection = Db.Open())
            {
                bool diningTablesExists = TableExists(connection, "DiningTables");
                bool diningTablesNewExists = TableExists(connection, "DiningTables_New");

                if (!diningTablesExists && diningTablesNewExists)
                {
                    DropDependentViews(connection);
                    connection.Execute("ALTER TABLE DiningTables_New RENAME TO DiningTables;");
                    EnsureDiningTableIndexes(connection);
                    diningTablesExists = true;
                }

                if (!diningTablesExists)
                {
                    CreateDiningTablesTable(connection, "DiningTables");
                    EnsureDiningTableIndexes(connection);
                    return;
                }

                string diningTablesSql = connection.ExecuteScalar<string>(
                    "SELECT sql FROM sqlite_master WHERE type = 'table' AND name = 'DiningTables';");

                if (!string.IsNullOrWhiteSpace(diningTablesSql) &&
                    diningTablesSql.Contains("SeatCapacity BETWEEN 1 AND 4"))
                {
                    DropDependentViews(connection);
                    connection.Execute("PRAGMA foreign_keys = OFF;");

                    try
                    {
                        connection.Execute(@"
CREATE TABLE DiningTables_New (
    TableId INTEGER PRIMARY KEY AUTOINCREMENT,
    TableCode TEXT NOT NULL UNIQUE,
    ColumnLetter TEXT NOT NULL,
    RowNumber INTEGER NOT NULL,
    CurrentTableStatusId INTEGER NOT NULL,
    SeatCapacity INTEGER NOT NULL CHECK (SeatCapacity >= 4),
    FOREIGN KEY (CurrentTableStatusId) REFERENCES TableStatus(TableStatusId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CHECK (length(ColumnLetter) = 1),
    CHECK (RowNumber BETWEEN 1 AND 6),
    CHECK (ColumnLetter IN ('A','B','C','D','E','F')),
    UNIQUE (ColumnLetter, RowNumber)
);

INSERT INTO DiningTables_New (TableId, TableCode, ColumnLetter, RowNumber, CurrentTableStatusId, SeatCapacity)
SELECT
    TableId,
    TableCode,
    ColumnLetter,
    RowNumber,
    CurrentTableStatusId,
    CASE
        WHEN SeatCapacity < 4 THEN 4
        ELSE SeatCapacity
    END
FROM DiningTables;

DROP TABLE DiningTables;

ALTER TABLE DiningTables_New RENAME TO DiningTables;

");
                        EnsureDiningTableIndexes(connection);
                    }
                    finally
                    {
                        connection.Execute("PRAGMA foreign_keys = ON;");
                    }
                }
            }
        }

        private static bool TableExists(Microsoft.Data.Sqlite.SqliteConnection connection, string tableName)
        {
            return connection.ExecuteScalar<long>(
                "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = @TableName;",
                new { TableName = tableName }) > 0;
        }

        private static void CreateDiningTablesTable(Microsoft.Data.Sqlite.SqliteConnection connection, string tableName)
        {
            connection.Execute($@"
CREATE TABLE {tableName} (
    TableId INTEGER PRIMARY KEY AUTOINCREMENT,
    TableCode TEXT NOT NULL UNIQUE,
    ColumnLetter TEXT NOT NULL,
    RowNumber INTEGER NOT NULL,
    CurrentTableStatusId INTEGER NOT NULL,
    SeatCapacity INTEGER NOT NULL CHECK (SeatCapacity >= 4),
    FOREIGN KEY (CurrentTableStatusId) REFERENCES TableStatus(TableStatusId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CHECK (length(ColumnLetter) = 1),
    CHECK (RowNumber BETWEEN 1 AND 6),
    CHECK (ColumnLetter IN ('A','B','C','D','E','F')),
    UNIQUE (ColumnLetter, RowNumber)
);");
        }

        private static void EnsureDiningTableIndexes(Microsoft.Data.Sqlite.SqliteConnection connection)
        {
            connection.Execute(@"
CREATE INDEX IF NOT EXISTS idx_diningtables_status
    ON DiningTables(CurrentTableStatusId);

CREATE INDEX IF NOT EXISTS idx_diningtables_location
    ON DiningTables(ColumnLetter, RowNumber);");
        }

        private static void DropDependentViews(Microsoft.Data.Sqlite.SqliteConnection connection)
        {
            connection.Execute(@"
DROP VIEW IF EXISTS vw_order_payment_summary;
DROP VIEW IF EXISTS vw_order_payment_summary_detailed;");
        }

        private static void EnsurePaymentViews()
        {
            using (var connection = Db.Open())
            {
                DropDependentViews(connection);
                connection.Execute(@"
CREATE VIEW vw_order_payment_summary AS
SELECT
    o.OrderId,
    ROUND(o.Total, 2) AS OrderTotal,
    ROUND(IFNULL(SUM(p.Amount), 0), 2) AS TotalPaid,
    ROUND(o.Total - IFNULL(SUM(p.Amount), 0), 2) AS BalanceRemaining,
    CASE
        WHEN ROUND(IFNULL(SUM(p.Amount), 0), 2) = 0 THEN 'Unpaid'
        WHEN ROUND(IFNULL(SUM(p.Amount), 0), 2) < ROUND(o.Total, 2) THEN 'Partially Paid'
        WHEN ROUND(IFNULL(SUM(p.Amount), 0), 2) = ROUND(o.Total, 2) THEN 'Paid'
        WHEN ROUND(IFNULL(SUM(p.Amount), 0), 2) > ROUND(o.Total, 2) THEN 'Overpaid'
        ELSE 'Unknown'
    END AS PaymentStatus
FROM Orders o
LEFT JOIN Payments p ON o.OrderId = p.OrderId
GROUP BY o.OrderId;

CREATE VIEW vw_order_payment_summary_detailed AS
SELECT
    o.OrderId,
    dt.TableCode,
    e.FirstName || ' ' || e.LastName AS ServerName,
    os.StatusName AS OrderStatus,
    ROUND(o.Total, 2) AS OrderTotal,
    ROUND(IFNULL(SUM(p.Amount), 0), 2) AS TotalPaid,
    ROUND(o.Total - IFNULL(SUM(p.Amount), 0), 2) AS BalanceRemaining,
    CASE
        WHEN ROUND(IFNULL(SUM(p.Amount), 0), 2) = 0 THEN 'Unpaid'
        WHEN ROUND(IFNULL(SUM(p.Amount), 0), 2) < ROUND(o.Total, 2) THEN 'Partially Paid'
        WHEN ROUND(IFNULL(SUM(p.Amount), 0), 2) = ROUND(o.Total, 2) THEN 'Paid'
        WHEN ROUND(IFNULL(SUM(p.Amount), 0), 2) > ROUND(o.Total, 2) THEN 'Overpaid'
        ELSE 'Unknown'
    END AS PaymentStatus
FROM Orders o
JOIN DiningTables dt ON o.TableId = dt.TableId
JOIN Employees e ON o.ServerEmployeeId = e.EmployeeId
JOIN OrderStatus os ON o.OrderStatusId = os.OrderStatusId
LEFT JOIN Payments p ON o.OrderId = p.OrderId
GROUP BY
    o.OrderId,
    dt.TableCode,
    e.FirstName,
    e.LastName,
    os.StatusName;");
            }
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

        private static void EnsureInventoryItems()
        {
            using (var connection = Db.Open())
            {
                connection.Execute(@"
INSERT OR IGNORE INTO InventoryItems (ItemName, UnitOfMeasure, QuantityOnHand, ReorderLevel, IsActive) VALUES
    ('Chicken Breast', 'lbs', 50, 15, 1),
    ('Ground Beef', 'lbs', 35, 10, 1),
    ('Lettuce', 'heads', 18, 8, 1),
    ('Tomatoes', 'lbs', 22, 8, 1),
    ('Cheddar Cheese', 'lbs', 14, 6, 1),
    ('French Fries', 'bags', 20, 7, 1),
    ('Sweet Tea', 'gallons', 12, 5, 1),
    ('To-Go Cups', 'cases', 4, 5, 1);");
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
    }
}
