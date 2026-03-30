SELECT o.OrderId, o.TableId, o.ServerEmployeeId, o.OrderStatusId
FROM Orders o
LEFT JOIN OrderItems oi ON o.OrderId = oi.OrderId
WHERE oi.OrderId IS NULL;

SELECT oi.OrderItemId, oi.OrderId
FROM OrderItems oi
LEFT JOIN Orders o ON oi.OrderId = o.OrderId
WHERE o.OrderId IS NULL;

SELECT oi.OrderItemId, oi.MenuItemId
FROM OrderItems oi
LEFT JOIN MenuItems mi ON oi.MenuItemId = mi.MenuItemId
WHERE mi.MenuItemId IS NULL;

SELECT o.OrderId, o.ServerEmployeeId
FROM Orders o
LEFT JOIN Employees e ON o.ServerEmployeeId = e.EmployeeId
WHERE e.EmployeeId IS NULL;

SELECT o.OrderId, o.TableId
FROM Orders o
LEFT JOIN DiningTables dt ON o.TableId = dt.TableId
WHERE dt.TableId IS NULL;

SELECT e.EmployeeId, e.RoleId
FROM Employees e
LEFT JOIN Roles r ON e.RoleId = r.RoleId
WHERE r.RoleId IS NULL;

SELECT MenuItemId, ItemName, Price
FROM MenuItems
WHERE Price < 0;

SELECT OrderItemId, OrderId, Qty
FROM OrderItems
WHERE Qty <= 0;

SELECT p.PaymentId, p.OrderId
FROM Payments p
LEFT JOIN Orders o ON p.OrderId = o.OrderId
WHERE o.OrderId IS NULL;

SELECT tc.TimeClockId, tc.EmployeeId
FROM TimeClock tc
LEFT JOIN Employees e ON tc.EmployeeId = e.EmployeeId
WHERE e.EmployeeId IS NULL;

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

SELECT
    oi.OrderId,
    ROUND(SUM(oi.Qty * oi.UnitPriceAtSale), 2) AS CalculatedSubtotal
FROM OrderItems oi
GROUP BY oi.OrderId
ORDER BY oi.OrderId;

SELECT
    o.OrderId,
    dt.TableCode,
    e.FirstName || ' ' || e.LastName AS ServerName,
    os.StatusName,
    mi.ItemName,
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

SELECT 'Roles' AS TableName, COUNT(*) AS RowCount FROM Roles
UNION ALL
SELECT 'Employees', COUNT(*) FROM Employees
UNION ALL
SELECT 'TableStatus', COUNT(*) FROM TableStatus
UNION ALL
SELECT 'DiningTables', COUNT(*) FROM DiningTables
UNION ALL
SELECT 'MenuCategories', COUNT(*) FROM MenuCategories
UNION ALL
SELECT 'MenuItems', COUNT(*) FROM MenuItems
UNION ALL
SELECT 'OrderStatus', COUNT(*) FROM OrderStatus
UNION ALL
SELECT 'Orders', COUNT(*) FROM Orders
UNION ALL
SELECT 'OrderItems', COUNT(*) FROM OrderItems
UNION ALL
SELECT 'Payments', COUNT(*) FROM Payments
UNION ALL
SELECT 'TimeClock', COUNT(*) FROM TimeClock;