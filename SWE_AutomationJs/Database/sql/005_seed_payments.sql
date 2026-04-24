PRAGMA foreign_keys = ON;

-- =========================================
-- Payments
-- =========================================

INSERT INTO Payments
(OrderId, Amount, PaymentMethod, ApprovedByEmployeeId, RefundFlag)
VALUES
    (1, 34.45, 'Card', 'E00004', 0),
    (2, 17.98, 'Cash', 'E00004', 0),
    (3, 23.49, 'Card', 'E00004', 0);