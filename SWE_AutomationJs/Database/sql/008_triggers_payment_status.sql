PRAGMA foreign_keys = ON;

DROP TRIGGER IF EXISTS trg_close_order_when_paid;

CREATE TRIGGER trg_close_order_when_paid
AFTER INSERT ON Payments
BEGIN
    UPDATE Orders
    SET OrderStatusId = (
        SELECT OrderStatusId
        FROM OrderStatus
        WHERE StatusName = 'Closed'
    ),
    ClosedAt = CURRENT_TIMESTAMP
    WHERE OrderId = NEW.OrderId
      AND (
          SELECT ROUND(IFNULL(SUM(Amount), 0), 2)
          FROM Payments
          WHERE OrderId = NEW.OrderId
      ) >= (
          SELECT ROUND(Total, 2)
          FROM Orders
          WHERE OrderId = NEW.OrderId
      );
END;