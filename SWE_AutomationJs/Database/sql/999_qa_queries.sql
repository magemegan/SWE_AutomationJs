PRAGMA foreign_keys = ON;

-- =========================================
-- ORPHAN / REFERENTIAL INTEGRITY CHECKS
-- =========================================

-- Orders without any order items
SELECT o.OrderId, o.TableId, o.ServerEmployeeId, o.OrderStatusId
FROM Orders o
LEFT JOIN OrderItems oi ON o.OrderId = oi.OrderId
WHERE oi.OrderId IS NULL;

-- OrderItems pointing to missing Orders
SELECT oi.OrderItemId, oi.OrderId
FROM OrderItems oi
LEFT JOIN Orders o ON oi.OrderId = o.OrderId
WHERE o.OrderId IS NULL;

-- OrderItems pointing to missing MenuItems
SELECT oi.OrderItemId, oi.MenuItemId
FROM OrderItems oi
LEFT JOIN MenuItems mi ON oi.MenuItemId = mi.MenuItemId
WHERE mi.MenuItemId IS NULL;

-- Orders pointing to missing Employees
SELECT o.OrderId, o.ServerEmployeeId
FROM Orders o
LEFT JOIN Employees e ON o.ServerEmployeeId = e.EmployeeId
WHERE e.EmployeeId IS NULL;

-- Orders pointing to missing DiningTables
SELECT o.OrderId, o.TableId
FROM Orders o
LEFT JOIN DiningTables dt ON o.TableId = dt.TableId
WHERE dt.TableId IS NULL;

-- Employees pointing to missing Roles
SELECT e.EmployeeId, e.RoleId
FROM Employees e
LEFT JOIN Roles r ON e.RoleId = r.RoleId
WHERE r.RoleId IS NULL;

-- Payments pointing to missing Orders
SELECT p.PaymentId, p.OrderId
FROM Payments p
LEFT JOIN Orders o ON p.OrderId = o.OrderId
WHERE o.OrderId IS NULL;

-- TimeClock rows pointing to missing Employees
SELECT tc.TimeClockId, tc.EmployeeId
FROM TimeClock tc
LEFT JOIN Employees e ON tc.EmployeeId = e.EmployeeId
WHERE e.EmployeeId IS NULL;

-- DiningTables pointing to missing TableStatus
SELECT dt.TableId, dt.CurrentTableStatusId
FROM DiningTables dt
LEFT JOIN TableStatus ts ON dt.CurrentTableStatusId = ts.TableStatusId
WHERE ts.TableStatusId IS NULL;

-- Waiter assignments pointing to missing Employees
SELECT wta.AssignmentId, wta.EmployeeId
FROM WaiterTableAssignments wta
LEFT JOIN Employees e ON wta.EmployeeId = e.EmployeeId
WHERE e.EmployeeId IS NULL;

-- Waiter assignments pointing to missing DiningTables
SELECT wta.AssignmentId, wta.TableId
FROM WaiterTableAssignments wta
LEFT JOIN DiningTables dt ON wta.TableId = dt.TableId
WHERE dt.TableId IS NULL;

-- TableStateEvents pointing to missing DiningTables
SELECT tse.TableStatusEventId, tse.TableId
FROM TableStateEvents tse
LEFT JOIN DiningTables dt ON tse.TableId = dt.TableId
WHERE dt.TableId IS NULL;

-- TableStateEvents pointing to missing Employees
SELECT tse.TableStatusEventId, tse.ChangedByEmployeeId
FROM TableStateEvents tse
LEFT JOIN Employees e ON tse.ChangedByEmployeeId = e.EmployeeId
WHERE e.EmployeeId IS NULL;

-- OrderStatusEvents pointing to missing Orders
SELECT ose.OrderStatusEventId, ose.OrderId
FROM OrderStatusEvents ose
LEFT JOIN Orders o ON ose.OrderId = o.OrderId
WHERE o.OrderId IS NULL;

-- OrderStatusEvents pointing to missing Employees
SELECT ose.OrderStatusEventId, ose.ChangedByEmployeeId
FROM OrderStatusEvents ose
LEFT JOIN Employees e ON ose.ChangedByEmployeeId = e.EmployeeId
WHERE e.EmployeeId IS NULL;

-- Payments approved by missing Employees
SELECT p.PaymentId, p.ApprovedByEmployeeId
FROM Payments p
LEFT JOIN Employees e ON p.ApprovedByEmployeeId = e.EmployeeId
WHERE p.ApprovedByEmployeeId IS NOT NULL
  AND e.EmployeeId IS NULL;

-- Refund approvals by missing Employees
SELECT p.PaymentId, p.RefundApprovedByEmployeeId
FROM Payments p
LEFT JOIN Employees e ON p.RefundApprovedByEmployeeId = e.EmployeeId
WHERE p.RefundApprovedByEmployeeId IS NOT NULL
  AND e.EmployeeId IS NULL;


-- =========================================
-- DATA VALIDATION CHECKS
-- =========================================

-- Negative menu prices
SELECT MenuItemId, ItemName, Price
FROM MenuItems
WHERE Price < 0;

-- Non-positive quantity
SELECT OrderItemId, OrderId, Qty
FROM OrderItems
WHERE Qty <= 0;

-- Invalid seat number
SELECT OrderItemId, OrderId, SeatNumber
FROM OrderItems
WHERE SeatNumber IS NOT NULL
  AND (SeatNumber < 1 OR SeatNumber > 4);

-- Invalid seat capacity
SELECT TableId, TableCode, SeatCapacity
FROM DiningTables
WHERE SeatCapacity < 1 OR SeatCapacity > 4;

-- Invalid party size
SELECT OrderId, PartySize
FROM Orders
WHERE PartySize IS NOT NULL
  AND (PartySize < 1 OR PartySize > 4);

-- Invalid table location
SELECT TableId, TableCode, ColumnLetter, RowNumber
FROM DiningTables
WHERE ColumnLetter NOT IN ('A','B','C','D','E','F')
   OR RowNumber NOT BETWEEN 1 AND 6;

-- Invalid payment method
SELECT PaymentId, PaymentMethod
FROM Payments
WHERE PaymentMethod NOT IN ('Cash', 'Card');

-- Negative payment amounts
SELECT PaymentId, OrderId, Amount
FROM Payments
WHERE Amount < 0;

-- Negative refund amounts
SELECT PaymentId, RefundAmount
FROM Payments
WHERE RefundAmount < 0;

-- Invalid refund records
SELECT PaymentId, RefundFlag, RefundAmount, RefundApprovedByEmployeeId, RefundApprovedAt
FROM Payments
WHERE
    (RefundFlag = 0 AND
        (RefundAmount <> 0.00 OR RefundApprovedByEmployeeId IS NOT NULL OR RefundApprovedAt IS NOT NULL))
    OR
    (RefundFlag = 1 AND
        (RefundApprovedByEmployeeId IS NULL OR RefundApprovedAt IS NULL));

-- Invalid TimeClock ranges
SELECT TimeClockId, EmployeeId, ClockIn, ClockOut
FROM TimeClock
WHERE ClockOut IS NOT NULL
  AND ClockOut < ClockIn;

-- Invalid waiter assignment date ranges
SELECT AssignmentId, EmployeeId, TableId, AssignedAt, UnassignedAt
FROM WaiterTableAssignments
WHERE UnassignedAt IS NOT NULL
  AND UnassignedAt < AssignedAt;

-- Invalid Orders timestamps
SELECT OrderId, OpenedAt, SubmittedAt, ReadyAt, DeliveredAt, ClosedAt
FROM Orders
WHERE
    (SubmittedAt IS NOT NULL AND SubmittedAt < OpenedAt)
    OR (ReadyAt IS NOT NULL AND ReadyAt < OpenedAt)
    OR (DeliveredAt IS NOT NULL AND DeliveredAt < OpenedAt)
    OR (ClosedAt IS NOT NULL AND ClosedAt < OpenedAt);


-- =========================================
-- DUPLICATE / UNIQUENESS CHECKS
-- =========================================

SELECT RoleName, COUNT(*) AS RoleCount
FROM Roles
GROUP BY RoleName
HAVING COUNT(*) > 1;

SELECT CategoryName, COUNT(*) AS CategoryCount
FROM MenuCategories
GROUP BY CategoryName
HAVING COUNT(*) > 1;

SELECT TableCode, COUNT(*) AS TableCodeCount
FROM DiningTables
GROUP BY TableCode
HAVING COUNT(*) > 1;

SELECT ColumnLetter, RowNumber, COUNT(*) AS LocationCount
FROM DiningTables
GROUP BY ColumnLetter, RowNumber
HAVING COUNT(*) > 1;

SELECT CategoryId, ItemName, COUNT(*) AS MenuItemCount
FROM MenuItems
GROUP BY CategoryId, ItemName
HAVING COUNT(*) > 1;


-- =========================================
-- BUSINESS RULE / CALCULATION CHECKS
-- =========================================

-- Recalculate subtotal from OrderItems
SELECT
    oi.OrderId,
    ROUND(SUM(oi.Qty * oi.UnitPriceAtSale), 2) AS CalculatedSubtotal
FROM OrderItems oi
GROUP BY oi.OrderId
ORDER BY oi.OrderId;

-- Compare stored subtotal/tax/total to recalculated values
SELECT
    o.OrderId,
    ROUND(o.Subtotal, 2) AS StoredSubtotal,
    ROUND(IFNULL(SUM(oi.Qty * oi.UnitPriceAtSale), 0), 2) AS CalculatedSubtotal,
    ROUND(o.Tax, 2) AS StoredTax,
    ROUND(IFNULL(SUM(oi.Qty * oi.UnitPriceAtSale), 0) * 0.07, 2) AS CalculatedTax,
    ROUND(o.Total, 2) AS StoredTotal,
    ROUND(IFNULL(SUM(oi.Qty * oi.UnitPriceAtSale), 0) * 1.07, 2) AS CalculatedTotal
FROM Orders o
LEFT JOIN OrderItems oi ON o.OrderId = oi.OrderId
GROUP BY o.OrderId
HAVING
    ROUND(o.Subtotal, 2) <> ROUND(IFNULL(SUM(oi.Qty * oi.UnitPriceAtSale), 0), 2)
    OR ROUND(o.Tax, 2) <> ROUND(IFNULL(SUM(oi.Qty * oi.UnitPriceAtSale), 0) * 0.07, 2)
    OR ROUND(o.Total, 2) <> ROUND(IFNULL(SUM(oi.Qty * oi.UnitPriceAtSale), 0) * 1.07, 2);

-- Payment totals by order
SELECT
    o.OrderId,
    ROUND(o.Total, 2) AS OrderTotal,
    ROUND(IFNULL(SUM(p.Amount), 0), 2) AS TotalPaid,
    ROUND(o.Total - IFNULL(SUM(p.Amount), 0), 2) AS BalanceRemaining
FROM Orders o
LEFT JOIN Payments p ON o.OrderId = p.OrderId
GROUP BY o.OrderId
ORDER BY o.OrderId;

-- Orders marked closed but not fully paid
SELECT
    o.OrderId,
    ROUND(o.Total, 2) AS OrderTotal,
    ROUND(IFNULL(SUM(p.Amount), 0), 2) AS TotalPaid,
    os.StatusName
FROM Orders o
JOIN OrderStatus os ON o.OrderStatusId = os.OrderStatusId
LEFT JOIN Payments p ON o.OrderId = p.OrderId
WHERE os.StatusName = 'Closed'
GROUP BY o.OrderId, o.Total, os.StatusName
HAVING ROUND(IFNULL(SUM(p.Amount), 0), 2) < ROUND(o.Total, 2);

-- Orders fully paid but not closed
SELECT
    o.OrderId,
    ROUND(o.Total, 2) AS OrderTotal,
    ROUND(IFNULL(SUM(p.Amount), 0), 2) AS TotalPaid,
    os.StatusName
FROM Orders o
JOIN OrderStatus os ON o.OrderStatusId = os.OrderStatusId
LEFT JOIN Payments p ON o.OrderId = p.OrderId
GROUP BY o.OrderId, o.Total, os.StatusName
HAVING ROUND(IFNULL(SUM(p.Amount), 0), 2) >= ROUND(o.Total, 2)
   AND os.StatusName <> 'Closed';


-- =========================================
-- DETAIL OUTPUT / MANUAL REVIEW
-- =========================================

SELECT
    o.OrderId,
    dt.TableCode,
    e.FirstName || ' ' || e.LastName AS ServerName,
    os.StatusName,
    mi.ItemName,
    oi.SeatNumber,
    oi.Qty,
    oi.UnitPriceAtSale,
    ROUND(oi.Qty * oi.UnitPriceAtSale, 2) AS LineTotal
FROM Orders o
JOIN DiningTables dt ON o.TableId = dt.TableId
JOIN Employees e ON o.ServerEmployeeId = e.EmployeeId
JOIN OrderStatus os ON o.OrderStatusId = os.OrderStatusId
JOIN OrderItems oi ON o.OrderId = oi.OrderId
JOIN MenuItems mi ON oi.MenuItemId = mi.MenuItemId
ORDER BY o.OrderId, oi.OrderItemId;

SELECT
    p.PaymentId,
    p.OrderId,
    p.Amount,
    p.PaymentMethod,
    p.PaidAt,
    p.RefundFlag,
    p.RefundAmount,
    p.RefundApprovedAt
FROM Payments p
ORDER BY p.PaymentId;

SELECT
    dt.TableId,
    dt.TableCode,
    dt.ColumnLetter,
    dt.RowNumber,
    dt.SeatCapacity,
    ts.StatusName AS CurrentStatus
FROM DiningTables dt
JOIN TableStatus ts ON dt.CurrentTableStatusId = ts.TableStatusId
ORDER BY dt.ColumnLetter, dt.RowNumber;


-- =========================================
-- ROW COUNTS
-- =========================================

SELECT 'Roles' AS TableName, COUNT(*) AS RowCount FROM Roles
UNION ALL
SELECT 'Employees', COUNT(*) FROM Employees
UNION ALL
SELECT 'TableStatus', COUNT(*) FROM TableStatus
UNION ALL
SELECT 'DiningTables', COUNT(*) FROM DiningTables
UNION ALL
SELECT 'TableStateEvents', COUNT(*) FROM TableStateEvents
UNION ALL
SELECT 'WaiterTableAssignments', COUNT(*) FROM WaiterTableAssignments
UNION ALL
SELECT 'MenuCategories', COUNT(*) FROM MenuCategories
UNION ALL
SELECT 'MenuItems', COUNT(*) FROM MenuItems
UNION ALL
SELECT 'OrderStatus', COUNT(*) FROM OrderStatus
UNION ALL
SELECT 'Orders', COUNT(*) FROM Orders
UNION ALL
SELECT 'OrderStatusEvents', COUNT(*) FROM OrderStatusEvents
UNION ALL
SELECT 'OrderItems', COUNT(*) FROM OrderItems
UNION ALL
SELECT 'Payments', COUNT(*) FROM Payments
UNION ALL
SELECT 'TimeClock', COUNT(*) FROM TimeClock
UNION ALL
SELECT 'InventoryItems', COUNT(*) FROM InventoryItems;