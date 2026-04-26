using Dapper;

namespace SWE_AutomationJs_UI_Design.Data
{
    public static class ReportRepository
    {
        public static int GetTotalOrdersToday()
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT COUNT(*)
FROM Orders
WHERE date(CreatedAt) = date('now');";

                return connection.ExecuteScalar<int>(sql);
            }
        }

        public static decimal GetRevenueToday()
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT IFNULL(SUM(Amount), 0)
FROM Payments
WHERE date(PaymentTime) = date('now');";

                return connection.ExecuteScalar<decimal>(sql);
            }
        }

        public static string GetMostPopularItem()
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT mi.ItemName
FROM OrderItems oi
INNER JOIN MenuItems mi ON mi.MenuItemId = oi.MenuItemId
GROUP BY mi.ItemName
ORDER BY COUNT(*) DESC
LIMIT 1;";

                string result = connection.ExecuteScalar<string>(sql);

                if (string.IsNullOrWhiteSpace(result))
                {
                    return "No sales yet";
                }

                return result;
            }
        }
    }
}