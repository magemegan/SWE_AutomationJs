PRAGMA foreign_keys = ON;

DROP TRIGGER IF EXISTS trg_orderitems_after_insert;
DROP TRIGGER IF EXISTS trg_orderitems_after_update;
DROP TRIGGER IF EXISTS trg_orderitems_after_delete;

CREATE TRIGGER trg_orderitems_after_insert
AFTER INSERT ON OrderItems
BEGIN
    UPDATE Orders
    SET Subtotal = (
        SELECT IFNULL(SUM(Qty * UnitPriceAtSale), 0)
        FROM OrderItems
        WHERE OrderId = NEW.OrderId
    ),
    Tax = (
        SELECT ROUND(IFNULL(SUM(Qty * UnitPriceAtSale), 0) * 0.07, 2)
        FROM OrderItems
        WHERE OrderId = NEW.OrderId
    ),
    Total = (
        SELECT ROUND(IFNULL(SUM(Qty * UnitPriceAtSale), 0) * 1.07, 2)
        FROM OrderItems
        WHERE OrderId = NEW.OrderId
    )
    WHERE OrderId = NEW.OrderId;
END;

CREATE TRIGGER trg_orderitems_after_update
AFTER UPDATE ON OrderItems
BEGIN
    UPDATE Orders
    SET Subtotal = (
        SELECT IFNULL(SUM(Qty * UnitPriceAtSale), 0)
        FROM OrderItems
        WHERE OrderId = NEW.OrderId
    ),
    Tax = (
        SELECT ROUND(IFNULL(SUM(Qty * UnitPriceAtSale), 0) * 0.07, 2)
        FROM OrderItems
        WHERE OrderId = NEW.OrderId
    ),
    Total = (
        SELECT ROUND(IFNULL(SUM(Qty * UnitPriceAtSale), 0) * 1.07, 2)
        FROM OrderItems
        WHERE OrderId = NEW.OrderId
    )
    WHERE OrderId = NEW.OrderId;
END;

CREATE TRIGGER trg_orderitems_after_delete
AFTER DELETE ON OrderItems
BEGIN
    UPDATE Orders
    SET Subtotal = (
        SELECT IFNULL(SUM(Qty * UnitPriceAtSale), 0)
        FROM OrderItems
        WHERE OrderId = OLD.OrderId
    ),
    Tax = (
        SELECT ROUND(IFNULL(SUM(Qty * UnitPriceAtSale), 0) * 0.07, 2)
        FROM OrderItems
        WHERE OrderId = OLD.OrderId
    ),
    Total = (
        SELECT ROUND(IFNULL(SUM(Qty * UnitPriceAtSale), 0) * 1.07, 2)
        FROM OrderItems
        WHERE OrderId = OLD.OrderId
    )
    WHERE OrderId = OLD.OrderId;
END;