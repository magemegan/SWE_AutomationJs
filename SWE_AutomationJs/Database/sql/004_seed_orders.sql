PRAGMA foreign_keys = ON;

-- =========================================
-- Order 1 (Open)
-- =========================================

INSERT INTO Orders
(TableId, ServerEmployeeId, OrderStatusId, PartySize)
VALUES
(1, 'E00003', 1, 2);

INSERT INTO OrderItems
(OrderId, MenuItemId, SeatNumber, Qty, UnitPriceAtSale, Notes)
VALUES
    (1, 1, 1, 2, 7.99, 'Extra sauce'),
    (1, 4, 2, 1, 12.49, NULL),
    (1, 8, 2, 2, 2.99, 'No ice');

-- =========================================
-- Order 2 (Submitted)
-- =========================================

INSERT INTO Orders
(TableId, ServerEmployeeId, OrderStatusId, PartySize)
VALUES
(2, 'E00003', 2, 1);

INSERT INTO OrderItems
(OrderId, MenuItemId, SeatNumber, Qty, UnitPriceAtSale, Notes)
VALUES
    (2, 3, 1, 1, 14.99, NULL),
    (2, 9, 1, 1, 2.99, NULL);

-- =========================================
-- Order 3 (Ready)
-- =========================================

INSERT INTO Orders
(TableId, ServerEmployeeId, OrderStatusId, PartySize)
VALUES
(3, 'E00003', 3, 2);

INSERT INTO OrderItems
(OrderId, MenuItemId, SeatNumber, Qty, UnitPriceAtSale, Notes)
VALUES
    (3, 5, 1, 1, 16.99, NULL),
    (3, 6, 2, 1, 6.50, NULL);