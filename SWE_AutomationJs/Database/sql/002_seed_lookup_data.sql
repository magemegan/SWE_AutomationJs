PRAGMA foreign_keys = ON;

INSERT OR IGNORE INTO Roles (RoleName) VALUES
    ('Admin'),
    ('Manager'),
    ('Server'),
    ('Cashier'),
    ('Kitchen');

INSERT OR IGNORE INTO TableStatus (StatusName) VALUES
    ('Available'),
    ('Occupied'),
    ('Reserved'),
    ('Dirty'),
    ('Closed');

INSERT OR IGNORE INTO OrderStatus (StatusName) VALUES
    ('Open'),
    ('Submitted'),
    ('Ready'),
    ('Closed'),
    ('Cancelled');

INSERT OR IGNORE INTO MenuCategories (CategoryName) VALUES
    ('Appetizers'),
    ('Entrees'),
    ('Desserts'),
    ('Drinks');