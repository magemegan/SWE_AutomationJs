PRAGMA foreign_keys = ON;

-- =========================================
-- Roles
-- =========================================

INSERT OR IGNORE INTO Roles (RoleName) VALUES
    ('Admin'),
    ('Manager'),
    ('Server'),
    ('Cashier'),
    ('Kitchen');

-- =========================================
-- Table Status
-- =========================================

INSERT OR IGNORE INTO TableStatus (StatusName) VALUES
    ('Available'),
    ('Occupied'),
    ('Dirty');

-- =========================================
-- Order Status
-- =========================================

INSERT OR IGNORE INTO OrderStatus (StatusName) VALUES
    ('Open'),
    ('Submitted'),
    ('Ready'),
    ('Closed'),
    ('Cancelled');

-- =========================================
-- Menu Categories
-- =========================================

INSERT OR IGNORE INTO MenuCategories (CategoryName) VALUES
    ('Appetizers'),
    ('Entrees'),
    ('Desserts'),
    ('Drinks');