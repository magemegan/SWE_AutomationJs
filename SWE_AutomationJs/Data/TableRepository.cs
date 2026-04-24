using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace SWE_AutomationJs_UI_Design.Data
{
    public sealed class TableStatusInfo
    {
        public int TableId { get; set; }
        public string TableCode { get; set; }
        public string StatusName { get; set; }

        public string UiStatus
        {
            get { return ToUiStatus(StatusName); }
        }

        private static string ToUiStatus(string statusName)
        {
            switch (statusName)
            {
                case "Available":
                    return "Open";
                case "Occupied":
                    return "Occupied";
                case "Dirty":
                    return "Needs Cleaning";
                default:
                    return statusName;
            }
        }
    }

    internal static class TableRepository
    {
        public static IReadOnlyList<TableStatusInfo> GetAllTableStatuses()
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT
    t.TableId,
    t.TableCode,
    s.StatusName
FROM DiningTables t
INNER JOIN TableStatus s ON s.TableStatusId = t.CurrentTableStatusId
ORDER BY t.TableId;";

                return connection.Query<TableStatusInfo>(sql).ToList();
            }
        }

        public static TableStatusInfo GetTableStatus(int tableId)
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT
    t.TableId,
    t.TableCode,
    s.StatusName
FROM DiningTables t
INNER JOIN TableStatus s ON s.TableStatusId = t.CurrentTableStatusId
WHERE t.TableId = @TableId;";

                return connection.QuerySingleOrDefault<TableStatusInfo>(sql, new { TableId = tableId });
            }
        }

        public static void SetStatus(int tableId, string targetStatusName, string changedByEmployeeId)
        {
            using (var connection = Db.Open())
            using (var transaction = connection.BeginTransaction())
            {
                int? currentStatusId = connection.ExecuteScalar<int?>(
                    "SELECT CurrentTableStatusId FROM DiningTables WHERE TableId = @TableId;",
                    new { TableId = tableId },
                    transaction);

                if (!currentStatusId.HasValue)
                {
                    return;
                }

                int nextStatusId = connection.ExecuteScalar<int>(
                    "SELECT TableStatusId FROM TableStatus WHERE StatusName = @StatusName;",
                    new { StatusName = targetStatusName },
                    transaction);

                if (currentStatusId.Value == nextStatusId)
                {
                    transaction.Commit();
                    return;
                }

                connection.Execute(@"
UPDATE DiningTables
SET CurrentTableStatusId = @NextStatusId
WHERE TableId = @TableId;",
                    new
                    {
                        TableId = tableId,
                        NextStatusId = nextStatusId
                    },
                    transaction);

                connection.Execute(@"
INSERT INTO TableStateEvents (TableId, OldTableStatusId, NewTableStatusId, ChangedByEmployeeId)
VALUES (@TableId, @OldStatusId, @NewStatusId, @ChangedByEmployeeId);",
                    new
                    {
                        TableId = tableId,
                        OldStatusId = currentStatusId.Value,
                        NewStatusId = nextStatusId,
                        ChangedByEmployeeId = changedByEmployeeId
                    },
                    transaction);

                transaction.Commit();
            }
        }

        public static IReadOnlyList<int> GetAssignedTableIds(string employeeId)
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT TableId
FROM WaiterTableAssignments
WHERE EmployeeId = @EmployeeId
  AND UnassignedAt IS NULL
ORDER BY TableId;";

                return connection.Query<int>(sql, new { EmployeeId = employeeId }).ToList();
            }
        }

        public static int AddTable()
        {
            using (var connection = Db.Open())
            {
                int nextTableId = connection.ExecuteScalar<int>("SELECT IFNULL(MAX(TableId), 0) + 1 FROM DiningTables;");
                int zeroBasedIndex = nextTableId - 1;
                string tableCode = "T" + nextTableId;
                string columnLetter = ((char)('A' + (zeroBasedIndex % 6))).ToString();
                int rowNumber = (zeroBasedIndex / 6) + 1;
                int availableStatusId = connection.ExecuteScalar<int>("SELECT TableStatusId FROM TableStatus WHERE StatusName = 'Available';");

                connection.Execute(@"
INSERT INTO DiningTables (TableId, TableCode, ColumnLetter, RowNumber, CurrentTableStatusId, SeatCapacity)
VALUES (@TableId, @TableCode, @ColumnLetter, @RowNumber, @StatusId, 4);",
                    new
                    {
                        TableId = nextTableId,
                        TableCode = tableCode,
                        ColumnLetter = columnLetter,
                        RowNumber = rowNumber,
                        StatusId = availableStatusId
                    });

                return nextTableId;
            }
        }

        public static void RemoveTable(int tableId)
        {
            using (var connection = Db.Open())
            {
                long activeOrders = connection.ExecuteScalar<long>(@"
SELECT COUNT(*)
FROM Orders o
INNER JOIN OrderStatus s ON s.OrderStatusId = o.OrderStatusId
WHERE o.TableId = @TableId
  AND s.StatusName IN ('Open', 'Submitted', 'Ready');",
                    new { TableId = tableId });

                if (activeOrders > 0)
                {
                    return;
                }

                connection.Execute("DELETE FROM WaiterTableAssignments WHERE TableId = @TableId AND UnassignedAt IS NULL;", new { TableId = tableId });
                connection.Execute("DELETE FROM DiningTables WHERE TableId = @TableId;", new { TableId = tableId });
            }
        }

        public static void AddSeat(int tableId)
        {
            using (var connection = Db.Open())
            {
                connection.Execute(@"
UPDATE DiningTables
SET SeatCapacity = SeatCapacity + 1
WHERE TableId = @TableId;",
                    new { TableId = tableId });
            }
        }

        public static void MergeTables(int primaryTableId, int secondaryTableId)
        {
            if (primaryTableId == secondaryTableId)
            {
                return;
            }

            using (var connection = Db.Open())
            using (var transaction = connection.BeginTransaction())
            {
                int secondarySeats = connection.ExecuteScalar<int>(
                    "SELECT SeatCapacity FROM DiningTables WHERE TableId = @TableId;",
                    new { TableId = secondaryTableId },
                    transaction);

                connection.Execute(@"
UPDATE DiningTables
SET SeatCapacity = SeatCapacity + @SeatsToAdd
WHERE TableId = @PrimaryTableId;",
                    new
                    {
                        PrimaryTableId = primaryTableId,
                        SeatsToAdd = secondarySeats
                    },
                    transaction);

                connection.Execute("DELETE FROM WaiterTableAssignments WHERE TableId = @TableId AND UnassignedAt IS NULL;", new { TableId = secondaryTableId }, transaction);
                connection.Execute("DELETE FROM DiningTables WHERE TableId = @TableId;", new { TableId = secondaryTableId }, transaction);
                transaction.Commit();
            }
        }
    }
}
