using System.Collections.Generic;
using Dapper;

namespace SWE_AutomationJs_UI_Design.Data
{
    public sealed class ManagerReportSummary
    {
        public int OrdersToday { get; set; }
        public decimal RevenueToday { get; set; }
        public decimal RevenueWeek { get; set; }
        public decimal RevenueMonth { get; set; }
        public string MostPopularItem { get; set; }
        public double AverageTurnaroundMinutes { get; set; }
        public double AveragePreparationMinutes { get; set; }
        public double AverageReadyToServedMinutes { get; set; }
    }

    public static class ReportRepository
    {
        public static ManagerReportSummary GetManagerSummary()
        {
            using (var db = Db.Open())
            {
                return new ManagerReportSummary
                {
                    OrdersToday = db.ExecuteScalar<int>(@"
SELECT COUNT(*)
FROM Orders
WHERE date(CreatedAt) = date('now');"),

                    RevenueToday = db.ExecuteScalar<decimal>(@"
SELECT IFNULL(SUM(TotalAmount), 0)
FROM Orders
WHERE date(CreatedAt) = date('now');"),

                    RevenueWeek = db.ExecuteScalar<decimal>(@"
SELECT IFNULL(SUM(TotalAmount), 0)
FROM Orders
WHERE date(CreatedAt) >= date('now', '-7 days');"),

                    RevenueMonth = db.ExecuteScalar<decimal>(@"
SELECT IFNULL(SUM(TotalAmount), 0)
FROM Orders
WHERE date(CreatedAt) >= date('now', '-30 days');"),

                    MostPopularItem = db.ExecuteScalar<string>(@"
SELECT mi.Name
FROM OrderItems oi
JOIN MenuItems mi ON mi.MenuItemId = oi.MenuItemId
GROUP BY mi.Name
ORDER BY COUNT(*) DESC
LIMIT 1;") ?? "No sales yet",

                    AverageTurnaroundMinutes = 0,
                    AveragePreparationMinutes = 0,
                    AverageReadyToServedMinutes = 0
                };
            }
        }

        public static IEnumerable<dynamic> GetRevenueByHourToday()
        {
            using (var db = Db.Open())
            {
                return db.Query(@"
SELECT 
    strftime('%H:00', CreatedAt) AS Hour,
    ROUND(SUM(TotalAmount), 2) AS Revenue
FROM Orders
WHERE date(CreatedAt) = date('now')
GROUP BY strftime('%H', CreatedAt)
ORDER BY Hour;");
            }
        }

        public static IEnumerable<dynamic> GetPopularItems(int limit)
        {
            using (var db = Db.Open())
            {
                return db.Query(@"
SELECT 
    mi.Name AS MenuItem,
    COUNT(*) AS TimesOrdered
FROM OrderItems oi
JOIN MenuItems mi ON mi.MenuItemId = oi.MenuItemId
GROUP BY mi.Name
ORDER BY TimesOrdered DESC
LIMIT @Limit;",
                    new { Limit = limit });
            }
        }

        public static IEnumerable<dynamic> GetPersonnelEfficiency()
        {
            using (var db = Db.Open())
            {
                return db.Query(@"
SELECT
    e.FullName AS Employee,
    COUNT(o.OrderId) AS OrdersHandled,
    ROUND(IFNULL(SUM(o.TotalAmount), 0), 2) AS RevenueHandled
FROM Employees e
LEFT JOIN Orders o ON o.ServerId = e.EmployeeId
GROUP BY e.EmployeeId, e.FullName
ORDER BY OrdersHandled DESC;");
            }
        }
    }
}