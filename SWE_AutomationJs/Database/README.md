DROP VIEW IF EXISTS vw_order_payment_summary;
DROP VIEW IF EXISTS vw_order_payment_summary_detailed;

-- =========================================
-- BASIC SUMMARY
-- =========================================

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

-- =========================================
-- DETAILED VIEW (MANAGER VIEW)
-- =========================================

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
