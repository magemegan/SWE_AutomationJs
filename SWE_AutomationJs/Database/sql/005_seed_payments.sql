PRAGMA foreign_keys = ON;

INSERT INTO Payments
(OrderId, Amount, PaymentMethod, ApprovedByEmployeeId, RefundFlag)
VALUES
    (1, 34.45, 'Card', 'E00004', 0),
    (2, 19.24, 'Cash', 'E00004', 0),
    (3, 25.14, 'Card', 'E00004', 0);