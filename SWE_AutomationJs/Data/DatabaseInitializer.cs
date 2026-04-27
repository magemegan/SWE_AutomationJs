using System;
using System.IO;
using Dapper;
using Microsoft.Data.Sqlite;

namespace SWE_AutomationJs_UI_Design.Data
{
    internal static class DatabaseInitializer
    {
        private static readonly string[] SqlFiles =
        {
            "001_schema.sql",
            "002_seed_lookup_data.sql",
            "003_seed_core_data.sql",
            "004_seed_orders.sql",
            "005_seed_payments.sql",
            "006_triggers_order_totals.sql",
            "007_views_payment_summary.sql",
            "008_triggers_payment_status.sql"
        };

        public static void Initialize()
        {
            EnsureDatabaseFolder();

            if (IsDatabaseMissingOrEmpty())
            {
                CreateDatabaseFromSqlFiles();
            }

            using (var connection = Db.Open())
            {
                EnsureMenuOptionTables(connection);
                FixExistingTables(connection);
            }
        }

        private static void EnsureDatabaseFolder()
        {
            string folder = Path.GetDirectoryName(Db.DatabasePath);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        private static bool IsDatabaseMissingOrEmpty()
        {
            if (!File.Exists(Db.DatabasePath))
            {
                return true;
            }

            FileInfo fileInfo = new FileInfo(Db.DatabasePath);

            if (fileInfo.Length == 0)
            {
                return true;
            }

            using (var connection = Db.Open())
            {
                int tableCount = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table';"
                );

                return tableCount == 0;
            }
        }

        private static void CreateDatabaseFromSqlFiles()
        {
            foreach (string fileName in SqlFiles)
            {
                RunSqlFile(fileName);
            }
        }

        private static void RunSqlFile(string fileName)
        {
            string sqlPath = FindSqlFile(fileName);
            string sql = File.ReadAllText(sqlPath);

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new InvalidOperationException("SQL file is empty: " + sqlPath);
            }

            using (var connection = Db.Open())
            {
                connection.Execute("PRAGMA foreign_keys = ON;");
                connection.Execute(sql);
            }
        }

        private static string FindSqlFile(string fileName)
        {
            DirectoryInfo current = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            while (current != null)
            {
                string candidate = Path.Combine(current.FullName, "Database", "sql", fileName);

                if (File.Exists(candidate))
                {
                    return candidate;
                }

                current = current.Parent;
            }

            throw new FileNotFoundException("Could not find SQL file: " + fileName);
        }

        private static void EnsureMenuOptionTables(SqliteConnection connection)
        {
            connection.Execute(@"
CREATE TABLE IF NOT EXISTS MenuItemOptionGroups (
    OptionGroupId INTEGER PRIMARY KEY AUTOINCREMENT,
    MenuItemId INTEGER NOT NULL,
    GroupName TEXT NOT NULL,
    IsRequired INTEGER NOT NULL DEFAULT 0 CHECK (IsRequired IN (0,1)),
    MinSelections INTEGER NOT NULL DEFAULT 0 CHECK (MinSelections >= 0),
    MaxSelections INTEGER NOT NULL DEFAULT 1 CHECK (MaxSelections >= 1),
    DisplayOrder INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (MenuItemId) REFERENCES MenuItems(MenuItemId)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    UNIQUE (MenuItemId, GroupName)
);

CREATE TABLE IF NOT EXISTS MenuItemOptions (
    OptionId INTEGER PRIMARY KEY AUTOINCREMENT,
    OptionGroupId INTEGER NOT NULL,
    OptionName TEXT NOT NULL,
    PriceDelta NUMERIC NOT NULL DEFAULT 0.00,
    DisplayOrder INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (OptionGroupId) REFERENCES MenuItemOptionGroups(OptionGroupId)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    UNIQUE (OptionGroupId, OptionName)
);

CREATE TABLE IF NOT EXISTS OrderItemOptions (
    OrderItemOptionId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderItemId INTEGER NOT NULL,
    OptionGroupName TEXT NOT NULL,
    OptionName TEXT NOT NULL,
    PriceDelta NUMERIC NOT NULL DEFAULT 0.00,
    DisplayOrder INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (OrderItemId) REFERENCES OrderItems(OrderItemId)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);
");
        }

        private static void FixExistingTables(SqliteConnection connection)
        {
            AddColumnIfMissing(connection, "MenuItemOptionGroups", "DisplayOrder", "INTEGER NOT NULL DEFAULT 0");
            AddColumnIfMissing(connection, "MenuItemOptions", "DisplayOrder", "INTEGER NOT NULL DEFAULT 0");
            AddColumnIfMissing(connection, "OrderItemOptions", "DisplayOrder", "INTEGER NOT NULL DEFAULT 0");

            AddColumnIfMissing(connection, "InventoryItems", "UnitOfMeasure", "TEXT NOT NULL DEFAULT 'each'");
            AddColumnIfMissing(connection, "InventoryItems", "ReorderLevel", "NUMERIC NOT NULL DEFAULT 0");
            AddColumnIfMissing(connection, "InventoryItems", "IsActive", "INTEGER NOT NULL DEFAULT 1");

            AddColumnIfMissing(connection, "Employees", "CreatedAt", "DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP");

            AddColumnIfMissing(connection, "Orders", "CreatedBy", "TEXT");
            AddColumnIfMissing(connection, "Payments", "CreatedBy", "TEXT");
            AddColumnIfMissing(connection, "Payments", "ApprovedByEmployeeId", "TEXT");
        }

        private static void AddColumnIfMissing(
            SqliteConnection connection,
            string tableName,
            string columnName,
            string columnDefinition)
        {
            int tableExists = connection.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = @TableName;",
                new { TableName = tableName }
            );

            if (tableExists == 0)
            {
                return;
            }

            int columnExists = connection.ExecuteScalar<int>(
                $"SELECT COUNT(*) FROM pragma_table_info('{tableName}') WHERE name = @ColumnName;",
                new { ColumnName = columnName }
            );

            if (columnExists == 0)
            {
                connection.Execute(
                    $"ALTER TABLE {tableName} ADD COLUMN {columnName} {columnDefinition};"
                );
            }
        }
    }
}