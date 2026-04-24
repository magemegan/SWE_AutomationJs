using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace SWE_AutomationJs_UI_Design.Data
{
    public sealed class PaymentHistoryEntry
    {
        public long OrderId { get; set; }
        public int TableId { get; set; }
        public string TableCode { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal BalanceRemaining { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? PaidAt { get; set; }
    }

    internal static class PaymentRepository
    {
        public static void RecordPayment(long orderId, decimal amount, string paymentMethod, string approvedByEmployeeId)
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
INSERT INTO Payments (OrderId, Amount, PaymentMethod, ApprovedByEmployeeId, RefundFlag)
VALUES (@OrderId, @Amount, @PaymentMethod, @ApprovedByEmployeeId, 0);";

                connection.Execute(sql, new
                {
                    OrderId = orderId,
                    Amount = amount,
                    PaymentMethod = paymentMethod,
                    ApprovedByEmployeeId = approvedByEmployeeId
                });
            }
        }

        public static IReadOnlyList<PaymentHistoryEntry> GetPaymentHistory()
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT
    s.OrderId,
    o.TableId,
    d.TableCode,
    s.OrderTotal,
    s.TotalPaid,
    s.BalanceRemaining,
    s.PaymentStatus,
    (
        SELECT p.PaymentMethod
        FROM Payments p
        WHERE p.OrderId = s.OrderId
        ORDER BY p.PaidAt DESC, p.PaymentId DESC
        LIMIT 1
    ) AS PaymentMethod,
    (
        SELECT p.PaidAt
        FROM Payments p
        WHERE p.OrderId = s.OrderId
        ORDER BY p.PaidAt DESC, p.PaymentId DESC
        LIMIT 1
    ) AS PaidAt
FROM vw_order_payment_summary s
INNER JOIN Orders o ON o.OrderId = s.OrderId
INNER JOIN DiningTables d ON d.TableId = o.TableId
WHERE s.TotalPaid > 0
ORDER BY PaidAt DESC, s.OrderId DESC;";

                return connection.Query<PaymentHistoryEntry>(sql).ToList();
            }
        }
    }
}
