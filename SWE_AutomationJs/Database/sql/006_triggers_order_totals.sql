PRAGMA foreign_keys = ON;

DROP TRIGGER IF EXISTS trg_orderitems_after_insert;
DROP TRIGGER IF EXISTS trg_orderitems_after_update;
DROP TRIGGER IF EXISTS trg_orderitems_after_delete;

-- =========================================
-- AFTER INSERT
-- =========================================

CREATE TRIGGER trg_orderitems_after_insert
AFTER INSERT ON OrderItems
BEGIN
    UPDATE Orders
    SET 
        Subtotal = (
            SELECT IFNULL(SUM(Qty * UnitPriceAtSale), 0)
            FROM OrderItems
            WHERE OrderId = NEW.OrderId
        ),
        Tax = ROUND((
            SELECT IFNULL(SUM(Qty * UnitPriceAtSale), 0)
            FROM OrderItems
            WHERE OrderId = NEW.OrderId
        ) * 0.07, 2),
        Total = ROUND((
            SELECT IFNULL(SUM(Qty * UnitPriceAtSale), 0)
            FROM OrderItems
            WHERE OrderId = NEW.OrderId
        ) * 1.07, 2)
    WHERE OrderId = NEW.OrderId;
END;

-- =========================================
-- AFTER UPDATE
-- =========================================

CREATE TRIGGER trg_orderitems_after_update
AFTER UPDATE ON OrderItems
BEGIN
    UPDATE Orders
    SET 
        Subtotal = (
            SELECT IFNULL(SUM(Qty * UnitPriceAtSale), 0)
            FROM OrderItems
            WHERE OrderId = NEW.OrderId
        ),
        Tax = ROUND((
            SELECT IFNULL(SUM(Qty * UnitPriceAtSale), 0)
            FROM OrderItems
            WHERE OrderId = NEW.OrderId
        ) * 0.07, 2),
        Total = ROUND((
            SELECT IFNULL(SUM(Qty * UnitPriceAtSale), 0)
            FROM OrderItems
            WHERE OrderId = NEW.OrderId
        ) * 1.07, 2)
    WHERE OrderId = NEW.OrderId;
END;

-- =========================================
-- AFTER DELETE
-- =========================================

CREATE TRIGGER trg_orderitems_after_delete
AFTER DELETE ON OrderItems
BEGIN
    UPDATE Orders
    SET 
        Subtotal = (
            SELECT IFNULL(SUM(Qty * UnitPriceAtSale), 0)
            FROM OrderItems
            WHERE OrderId = OLD.OrderId
        ),
        Tax = ROUND((
            SELECT IFNULL(SUM(Qty * UnitPriceAtSale), 0)
            FROM OrderItems
            WHERE OrderId = OLD.OrderId
        ) * 0.07, 2),
        Total = ROUND((
            SELECT IFNULL(SUM(Qty * UnitPriceAtSale), 0)
            FROM OrderItems
            WHERE OrderId = OLD.OrderId
        ) * 1.07, 2)
    WHERE OrderId = OLD.OrderId;
END;