using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace SWE_AutomationJs_UI_Design.Data
{
    public sealed class PaymentHistoryEntry
    {
        public long OrderId { get; set; }
        public long? PaymentId { get; set; }
        public int TableId { get; set; }
        public string TableCode { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalRefunded { get; set; }
        public decimal NetPaid { get; set; }
        public decimal BalanceRemaining { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? PaidAt { get; set; }
        public bool RefundFlag { get; set; }
        public decimal RefundAmount { get; set; }
        public string RefundApprovedByEmployeeId { get; set; }
        public DateTime? RefundApprovedAt { get; set; }
    }

    internal static class PaymentRepository
    {
        public static void RecordPayment(long orderId, decimal amount, string paymentMethod, string approvedByEmployeeId)
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
INSERT INTO Payments (OrderId, Amount, PaymentMethod, ApprovedByEmployeeId, RefundFlag, RefundAmount)
VALUES (@OrderId, @Amount, @PaymentMethod, @ApprovedByEmployeeId, 0, 0);";

                connection.Execute(sql, new
                {
                    OrderId = orderId,
                    Amount = amount,
                    PaymentMethod = paymentMethod,
                    ApprovedByEmployeeId = approvedByEmployeeId
                });
            }
        }

        public static void RecordRefund(long paymentId, decimal refundAmount, string approvedByEmployeeId)
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
UPDATE Payments
SET
    RefundFlag = 1,
    RefundAmount = @RefundAmount,
    RefundApprovedByEmployeeId = @ApprovedByEmployeeId,
    RefundApprovedAt = CURRENT_TIMESTAMP
WHERE PaymentId = @PaymentId
  AND IFNULL(RefundFlag, 0) = 0
  AND @RefundAmount >= 0
  AND @RefundAmount <= Amount;";

                int rowsAffected = connection.Execute(sql, new
                {
                    PaymentId = paymentId,
                    RefundAmount = refundAmount,
                    ApprovedByEmployeeId = approvedByEmployeeId
                });

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("Refund could not be recorded. The payment may already be refunded or the refund amount is invalid.");
                }
            }
        }

        public static IReadOnlyList<PaymentHistoryEntry> GetPaymentHistory()
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT
    s.OrderId,
    (
        SELECT p.PaymentId
        FROM Payments p
        WHERE p.OrderId = s.OrderId
          AND IFNULL(p.RefundFlag, 0) = 0
        ORDER BY p.PaidAt DESC, p.PaymentId DESC
        LIMIT 1
    ) AS PaymentId,
    o.TableId,
    d.TableCode,
    s.OrderTotal,
    s.TotalPaid,
    s.TotalRefunded,
    s.NetPaid,
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
    ) AS PaidAt,
    (
        SELECT IFNULL(p.RefundFlag, 0)
        FROM Payments p
        WHERE p.OrderId = s.OrderId
        ORDER BY p.PaidAt DESC, p.PaymentId DESC
        LIMIT 1
    ) AS RefundFlag,
    (
        SELECT IFNULL(p.RefundAmount, 0)
        FROM Payments p
        WHERE p.OrderId = s.OrderId
        ORDER BY p.PaidAt DESC, p.PaymentId DESC
        LIMIT 1
    ) AS RefundAmount,
    (
        SELECT p.RefundApprovedByEmployeeId
        FROM Payments p
        WHERE p.OrderId = s.OrderId
        ORDER BY p.PaidAt DESC, p.PaymentId DESC
        LIMIT 1
    ) AS RefundApprovedByEmployeeId,
    (
        SELECT p.RefundApprovedAt
        FROM Payments p
        WHERE p.OrderId = s.OrderId
        ORDER BY p.PaidAt DESC, p.PaymentId DESC
        LIMIT 1
    ) AS RefundApprovedAt
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