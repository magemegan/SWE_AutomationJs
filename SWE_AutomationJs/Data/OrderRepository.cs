using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;

namespace SWE_AutomationJs_UI_Design.Data
{
    public sealed class OrderHeader
    {
        public long OrderId { get; set; }
        public int TableId { get; set; }
        public string TableCode { get; set; }
        public string ServerEmployeeId { get; set; }
        public string ServerDisplayName { get; set; }
        public string StatusName { get; set; }
        public DateTime OpenedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ReadyAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }

    public sealed class OrderLine
    {
        public long OrderItemId { get; set; }
        public long OrderId { get; set; }
        public int MenuItemId { get; set; }
        public string ItemName { get; set; }
        public int? SeatNumber { get; set; }
        public int Qty { get; set; }
        public decimal UnitPriceAtSale { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public sealed class MenuCatalogItem
    {
        public int MenuItemId { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }

        public string DisplayText
        {
            get { return string.Format("{0} ${1:F2}", ItemName, Price); }
        }
    }

    public sealed class MenuOptionGroup
    {
        public int OptionGroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsRequired { get; set; }
        public int MinSelections { get; set; }
        public int MaxSelections { get; set; }
        public int DisplayOrder { get; set; }
        public List<MenuOptionChoice> Options { get; } = new List<MenuOptionChoice>();
    }

    public sealed class MenuOptionChoice
    {
        public int OptionId { get; set; }
        public int OptionGroupId { get; set; }
        public string OptionName { get; set; }
        public decimal PriceDelta { get; set; }
        public int DisplayOrder { get; set; }

        public string DisplayText
        {
            get
            {
                return PriceDelta > 0
                    ? string.Format("{0} (+${1:F2})", OptionName, PriceDelta)
                    : OptionName;
            }
        }

        public override string ToString()
        {
            return DisplayText;
        }
    }

    public sealed class SelectedMenuOption
    {
        public string GroupName { get; set; }
        public string OptionName { get; set; }
        public decimal PriceDelta { get; set; }
        public int DisplayOrder { get; set; }
    }

    public static class OrderRepository
    {
        public static long CreateDraftOrder(int tableId, string serverEmployeeId, int? partySize)
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
INSERT INTO Orders (TableId, ServerEmployeeId, OrderStatusId, PartySize)
VALUES (
    @TableId,
    @ServerEmployeeId,
    (SELECT OrderStatusId FROM OrderStatus WHERE StatusName = 'Open'),
    @PartySize
);
SELECT last_insert_rowid();";

                return connection.ExecuteScalar<long>(sql, new
                {
                    TableId = tableId,
                    ServerEmployeeId = serverEmployeeId,
                    PartySize = partySize
                });
            }
        }

        public static long GetOrCreateDraftOrder(int tableId, string serverEmployeeId, int? partySize)
        {
            long? existingOrderId = GetLatestActiveOrderIdForTable(tableId);
            return existingOrderId ?? CreateDraftOrder(tableId, serverEmployeeId, partySize);
        }

        public static void AddItem(long orderId, int menuItemId, int qty, int? seatNumber, string notes)
        {
            AddItem(orderId, menuItemId, qty, seatNumber, notes, null);
        }

        public static void AddItem(long orderId, int menuItemId, int qty, int? seatNumber, string notes, IReadOnlyList<SelectedMenuOption> selectedOptions)
        {
            using (var connection = Db.Open())
            using (var transaction = connection.BeginTransaction())
            {
                double? basePrice = connection.ExecuteScalar<double?>(@"
SELECT Price + 0.0
FROM MenuItems
WHERE MenuItemId = @MenuItemId
  AND IsActive = 1;",
                    new { MenuItemId = menuItemId },
                    transaction);

                if (!basePrice.HasValue)
                {
                    throw new InvalidOperationException("Menu item not found or inactive.");
                }

                decimal optionTotal = selectedOptions != null
                    ? selectedOptions.Sum(x => x.PriceDelta)
                    : 0m;

                const string sql = @"
INSERT INTO OrderItems (OrderId, MenuItemId, SeatNumber, Qty, UnitPriceAtSale, Notes)
VALUES
    (@OrderId, @MenuItemId, @SeatNumber, @Qty, @UnitPriceAtSale, @Notes);
SELECT last_insert_rowid();";

                long orderItemId = connection.ExecuteScalar<long>(sql, new
                {
                    OrderId = orderId,
                    MenuItemId = menuItemId,
                    Qty = qty,
                    SeatNumber = seatNumber,
                    UnitPriceAtSale = Convert.ToDecimal(basePrice.Value) + optionTotal,
                    Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim()
                }, transaction);

                if (selectedOptions != null)
                {
                    foreach (SelectedMenuOption option in selectedOptions)
                    {
                        connection.Execute(@"
INSERT INTO OrderItemOptions (OrderItemId, OptionGroupName, OptionName, PriceDelta, DisplayOrder)
VALUES (@OrderItemId, @OptionGroupName, @OptionName, @PriceDelta, @DisplayOrder);",
                            new
                            {
                                OrderItemId = orderItemId,
                                OptionGroupName = option.GroupName,
                                OptionName = option.OptionName,
                                PriceDelta = option.PriceDelta,
                                DisplayOrder = option.DisplayOrder
                            },
                            transaction);
                    }
                }

                RecalculateTotals(connection, orderId, transaction);
                transaction.Commit();
            }
        }

        public static void RemoveItem(long orderItemId)
        {
            using (var connection = Db.Open())
            {
                const string lookupSql = "SELECT OrderId FROM OrderItems WHERE OrderItemId = @OrderItemId;";
                long? orderId = connection.ExecuteScalar<long?>(lookupSql, new { OrderItemId = orderItemId });
                if (!orderId.HasValue)
                {
                    return;
                }

                connection.Execute("DELETE FROM OrderItems WHERE OrderItemId = @OrderItemId;", new { OrderItemId = orderItemId });
                RecalculateTotals(connection, orderId.Value);
            }
        }

        public static IReadOnlyList<MenuOptionGroup> GetOptionGroups(int menuItemId)
        {
            using (var connection = Db.Open())
            {
                List<MenuOptionGroup> groups = connection.Query<MenuOptionGroup>(@"
SELECT
    OptionGroupId,
    GroupName,
    IsRequired,
    MinSelections,
    MaxSelections,
    DisplayOrder
FROM MenuItemOptionGroups
WHERE MenuItemId = @MenuItemId
ORDER BY DisplayOrder, OptionGroupId;",
                    new { MenuItemId = menuItemId }).ToList();

                if (groups.Count == 0)
                {
                    return groups;
                }

                Dictionary<int, MenuOptionGroup> groupLookup = groups.ToDictionary(x => x.OptionGroupId);
                foreach (MenuOptionChoice option in connection.Query<MenuOptionChoice>(@"
SELECT
    OptionId,
    OptionGroupId,
    OptionName,
    PriceDelta + 0.0 AS PriceDelta,
    DisplayOrder
FROM MenuItemOptions
WHERE OptionGroupId IN @GroupIds
ORDER BY DisplayOrder, OptionId;",
                    new { GroupIds = groupLookup.Keys.ToArray() }))
                {
                    groupLookup[option.OptionGroupId].Options.Add(option);
                }

                return groups;
            }
        }

        public static void SubmitToKitchen(long orderId, string changedByEmployeeId)
        {
            TransitionStatus(orderId, "Open", "Submitted", changedByEmployeeId, "SubmittedAt");
        }

        public static void MarkReady(long orderId, string changedByEmployeeId)
        {
            TransitionStatus(orderId, "Submitted", "Ready", changedByEmployeeId, "ReadyAt");
        }

        public static void MarkClosed(long orderId, string changedByEmployeeId)
        {
            TransitionStatus(orderId, "Ready", "Closed", changedByEmployeeId, "ClosedAt");
        }

        public static OrderHeader GetHeader(long orderId)
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT
    o.OrderId,
    o.TableId,
    t.TableCode,
    o.ServerEmployeeId,
    e.FirstName || ' ' || e.LastName AS ServerDisplayName,
    s.StatusName,
    o.OpenedAt,
    o.SubmittedAt,
    o.ReadyAt,
    o.DeliveredAt,
    o.ClosedAt,
    o.Subtotal + 0.0 AS Subtotal,
    o.Tax + 0.0 AS Tax,
    o.Total + 0.0 AS Total
FROM Orders o
INNER JOIN DiningTables t ON t.TableId = o.TableId
INNER JOIN Employees e ON e.EmployeeId = o.ServerEmployeeId
INNER JOIN OrderStatus s ON s.OrderStatusId = o.OrderStatusId
WHERE o.OrderId = @OrderId;";

                return connection.QuerySingleOrDefault<OrderHeader>(sql, new { OrderId = orderId });
            }
        }

        public static long? GetOpenOrderIdForTable(int tableId)
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT o.OrderId
FROM Orders o
INNER JOIN OrderStatus s ON s.OrderStatusId = o.OrderStatusId
WHERE o.TableId = @TableId
  AND s.StatusName = 'Open'
ORDER BY o.OpenedAt DESC, o.OrderId DESC
LIMIT 1;";

                return connection.ExecuteScalar<long?>(sql, new { TableId = tableId });
            }
        }

        public static long? GetLatestActiveOrderIdForTable(int tableId)
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT o.OrderId
FROM Orders o
INNER JOIN OrderStatus s ON s.OrderStatusId = o.OrderStatusId
WHERE o.TableId = @TableId
  AND s.StatusName IN ('Open', 'Submitted', 'Ready')
ORDER BY o.OpenedAt DESC, o.OrderId DESC
LIMIT 1;";

                return connection.ExecuteScalar<long?>(sql, new { TableId = tableId });
            }
        }

        public static IReadOnlyList<OrderHeader> GetKitchenQueue()
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT
    o.OrderId,
    o.TableId,
    t.TableCode,
    o.ServerEmployeeId,
    e.FirstName || ' ' || e.LastName AS ServerDisplayName,
    s.StatusName,
    o.OpenedAt,
    o.SubmittedAt,
    o.ReadyAt,
    o.DeliveredAt,
    o.ClosedAt,
    o.Subtotal + 0.0 AS Subtotal,
    o.Tax + 0.0 AS Tax,
    o.Total + 0.0 AS Total
FROM Orders o
INNER JOIN DiningTables t ON t.TableId = o.TableId
INNER JOIN Employees e ON e.EmployeeId = o.ServerEmployeeId
INNER JOIN OrderStatus s ON s.OrderStatusId = o.OrderStatusId
WHERE s.StatusName IN ('Submitted', 'Ready')
ORDER BY
    CASE s.StatusName WHEN 'Submitted' THEN 0 ELSE 1 END,
    o.SubmittedAt,
    o.OpenedAt;";

                return connection.Query<OrderHeader>(sql).ToList();
            }
        }

        public static IReadOnlyList<OrderLine> GetItems(long orderId)
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT
    oi.OrderItemId,
    oi.OrderId,
    oi.MenuItemId,
    m.ItemName,
    oi.SeatNumber,
    oi.Qty,
    oi.UnitPriceAtSale + 0.0 AS UnitPriceAtSale,
    oi.Notes,
    oi.CreatedAt
FROM OrderItems oi
INNER JOIN MenuItems m ON m.MenuItemId = oi.MenuItemId
WHERE oi.OrderId = @OrderId
ORDER BY oi.CreatedAt, oi.OrderItemId;";

                return connection.Query<OrderLine>(sql, new { OrderId = orderId }).ToList();
            }
        }

        public static IReadOnlyList<MenuCatalogItem> GetMenuCatalog()
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT
    m.MenuItemId,
    c.CategoryName,
    m.ItemName,
    m.Price + 0.0 AS Price
FROM MenuItems m
INNER JOIN MenuCategories c ON c.CategoryId = m.CategoryId
WHERE m.IsActive = 1
ORDER BY c.CategoryName, m.ItemName;";

                return connection.Query<MenuCatalogItem>(sql).ToList();
            }
        }

        private static void TransitionStatus(long orderId, string expectedCurrentStatus, string nextStatus, string changedByEmployeeId, string timestampColumn)
        {
            using (var connection = Db.Open())
            using (var transaction = connection.BeginTransaction())
            {
                const string statusLookupSql = @"
SELECT s.StatusName
FROM Orders o
INNER JOIN OrderStatus s ON s.OrderStatusId = o.OrderStatusId
WHERE o.OrderId = @OrderId;";

                string currentStatus = connection.ExecuteScalar<string>(statusLookupSql, new { OrderId = orderId }, transaction);
                if (currentStatus == null)
                {
                    throw new InvalidOperationException("Order was not found.");
                }

                if (!string.Equals(currentStatus, expectedCurrentStatus, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Order is not in the expected status.");
                }

                string updateSql = @"
UPDATE Orders
SET OrderStatusId = (SELECT OrderStatusId FROM OrderStatus WHERE StatusName = @NextStatus),
    " + timestampColumn + @" = CURRENT_TIMESTAMP
WHERE OrderId = @OrderId;";

                connection.Execute(updateSql, new
                {
                    OrderId = orderId,
                    NextStatus = nextStatus
                }, transaction);

                const string eventSql = @"
INSERT INTO OrderStatusEvents (OrderId, OldOrderStatusId, NewOrderStatusId, ChangedByEmployeeId)
VALUES (
    @OrderId,
    (SELECT OrderStatusId FROM OrderStatus WHERE StatusName = @CurrentStatus),
    (SELECT OrderStatusId FROM OrderStatus WHERE StatusName = @NextStatus),
    @ChangedByEmployeeId
);";

                connection.Execute(eventSql, new
                {
                    OrderId = orderId,
                    CurrentStatus = currentStatus,
                    NextStatus = nextStatus,
                    ChangedByEmployeeId = changedByEmployeeId
                }, transaction);

                transaction.Commit();
            }
        }

        private static void RecalculateTotals(Microsoft.Data.Sqlite.SqliteConnection connection, long orderId)
        {
            RecalculateTotals(connection, orderId, null);
        }

        private static void RecalculateTotals(Microsoft.Data.Sqlite.SqliteConnection connection, long orderId, Microsoft.Data.Sqlite.SqliteTransaction transaction)
        {
            double subtotalValue = connection.ExecuteScalar<double>(
                "SELECT COALESCE(SUM(Qty * UnitPriceAtSale), 0.0) FROM OrderItems WHERE OrderId = @OrderId;",
                new { OrderId = orderId },
                transaction);
            decimal subtotal = Convert.ToDecimal(subtotalValue);

            decimal taxRate = GetTaxRate();
            decimal tax = decimal.Round(subtotal * taxRate, 2, MidpointRounding.AwayFromZero);
            decimal total = subtotal + tax;

            const string sql = @"
UPDATE Orders
SET Subtotal = @Subtotal,
    Tax = @Tax,
    Total = @Total
WHERE OrderId = @OrderId;";

            connection.Execute(sql, new
            {
                OrderId = orderId,
                Subtotal = subtotal,
                Tax = tax,
                Total = total
            }, transaction);
        }

        private static decimal GetTaxRate()
        {
            string configuredRate = ConfigurationManager.AppSettings["Tax.Rate"];
            decimal parsedRate;
            if (!decimal.TryParse(configuredRate, NumberStyles.Number, CultureInfo.InvariantCulture, out parsedRate))
            {
                return 0.08m;
            }

            return parsedRate;
        }
    }
}
