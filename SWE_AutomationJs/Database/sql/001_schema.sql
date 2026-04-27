PRAGMA foreign_keys = ON;

-- =========================================
-- Lookup Tables
-- =========================================

CREATE TABLE IF NOT EXISTS Roles (
    RoleId INTEGER PRIMARY KEY AUTOINCREMENT,
    RoleName TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS TableStatus (
    TableStatusId INTEGER PRIMARY KEY AUTOINCREMENT,
    StatusName TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS OrderStatus (
    OrderStatusId INTEGER PRIMARY KEY AUTOINCREMENT,
    StatusName TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS MenuCategories (
    CategoryId INTEGER PRIMARY KEY AUTOINCREMENT,
    CategoryName TEXT NOT NULL UNIQUE
);

-- =========================================
-- Employees
-- =========================================

CREATE TABLE IF NOT EXISTS Employees (
    EmployeeId TEXT PRIMARY KEY,
    RoleId INTEGER NOT NULL,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    PasswordHash TEXT NOT NULL,
    IsActive INTEGER NOT NULL DEFAULT 1 CHECK (IsActive IN (0,1)),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CHECK (length(EmployeeId) = 6)
);

CREATE TABLE IF NOT EXISTS TimeClock (
    TimeClockId INTEGER PRIMARY KEY AUTOINCREMENT,
    EmployeeId TEXT NOT NULL,
    ClockIn DATETIME NOT NULL,
    ClockOut DATETIME,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId),
    CHECK (ClockOut IS NULL OR ClockOut >= ClockIn)
);

-- =========================================
-- Dining Tables
-- =========================================

CREATE TABLE IF NOT EXISTS DiningTables (
    TableId INTEGER PRIMARY KEY AUTOINCREMENT,
    TableCode TEXT NOT NULL UNIQUE,
    ColumnLetter TEXT NOT NULL,
    RowNumber INTEGER NOT NULL,
    CurrentTableStatusId INTEGER NOT NULL,
    SeatCapacity INTEGER NOT NULL CHECK (SeatCapacity >= 1),
    FOREIGN KEY (CurrentTableStatusId) REFERENCES TableStatus(TableStatusId),
    UNIQUE (ColumnLetter, RowNumber)
);

CREATE TABLE IF NOT EXISTS TableStateEvents (
    TableStatusEventId INTEGER PRIMARY KEY AUTOINCREMENT,
    TableId INTEGER NOT NULL,
    OldTableStatusId INTEGER,
    NewTableStatusId INTEGER NOT NULL,
    ChangedByEmployeeId TEXT NOT NULL,
    ChangedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (TableId) REFERENCES DiningTables(TableId),
    FOREIGN KEY (ChangedByEmployeeId) REFERENCES Employees(EmployeeId)
);

CREATE TABLE IF NOT EXISTS WaiterTableAssignments (
    AssignmentId INTEGER PRIMARY KEY AUTOINCREMENT,
    EmployeeId TEXT NOT NULL,
    TableId INTEGER NOT NULL,
    AssignedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UnassignedAt DATETIME,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId),
    FOREIGN KEY (TableId) REFERENCES DiningTables(TableId)
);

-- =========================================
-- Menu
-- =========================================

CREATE TABLE IF NOT EXISTS MenuItems (
    MenuItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    CategoryId INTEGER NOT NULL,
    ItemName TEXT NOT NULL,
    Price NUMERIC NOT NULL CHECK (Price >= 0),
    IsActive INTEGER NOT NULL DEFAULT 1,
    FOREIGN KEY (CategoryId) REFERENCES MenuCategories(CategoryId)
);

-- 🔴 FIX: Missing tables (CRITICAL)

CREATE TABLE IF NOT EXISTS MenuItemOptionGroups (
    OptionGroupId INTEGER PRIMARY KEY AUTOINCREMENT,
    MenuItemId INTEGER NOT NULL,
    GroupName TEXT NOT NULL,
    IsRequired INTEGER NOT NULL DEFAULT 0,
    MinSelections INTEGER NOT NULL DEFAULT 0,
    MaxSelections INTEGER NOT NULL DEFAULT 1,
    FOREIGN KEY (MenuItemId) REFERENCES MenuItems(MenuItemId)
);

CREATE TABLE IF NOT EXISTS MenuItemOptions (
    OptionId INTEGER PRIMARY KEY AUTOINCREMENT,
    OptionGroupId INTEGER NOT NULL,
    OptionName TEXT NOT NULL,
    PriceDelta NUMERIC NOT NULL DEFAULT 0.00,
    FOREIGN KEY (OptionGroupId) REFERENCES MenuItemOptionGroups(OptionGroupId)
);

-- =========================================
-- Orders
-- =========================================

CREATE TABLE IF NOT EXISTS Orders (
    OrderId INTEGER PRIMARY KEY AUTOINCREMENT,
    TableId INTEGER NOT NULL,
    ServerEmployeeId TEXT NOT NULL,
    OrderStatusId INTEGER NOT NULL,
    PartySize INTEGER,
    OpenedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    SubmittedAt DATETIME,
    ReadyAt DATETIME,
    DeliveredAt DATETIME,
    ClosedAt DATETIME,
    Subtotal NUMERIC DEFAULT 0,
    Tax NUMERIC DEFAULT 0,
    Total NUMERIC DEFAULT 0,
    FOREIGN KEY (TableId) REFERENCES DiningTables(TableId),
    FOREIGN KEY (ServerEmployeeId) REFERENCES Employees(EmployeeId),
    FOREIGN KEY (OrderStatusId) REFERENCES OrderStatus(OrderStatusId)
);

CREATE TABLE IF NOT EXISTS OrderItems (
    OrderItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    MenuItemId INTEGER NOT NULL,
    SeatNumber INTEGER,
    Qty INTEGER NOT NULL,
    UnitPriceAtSale NUMERIC NOT NULL,
    Notes TEXT,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (MenuItemId) REFERENCES MenuItems(MenuItemId)
);

-- 🔴 FIX: Missing table

CREATE TABLE IF NOT EXISTS OrderItemOptions (
    OrderItemOptionId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderItemId INTEGER NOT NULL,
    OptionGroupName TEXT NOT NULL,
    OptionName TEXT NOT NULL,
    PriceDelta NUMERIC NOT NULL DEFAULT 0.00,
    FOREIGN KEY (OrderItemId) REFERENCES OrderItems(OrderItemId)
);

-- =========================================
-- Payments
-- =========================================

CREATE TABLE IF NOT EXISTS Payments (
    PaymentId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    Amount NUMERIC NOT NULL,
    PaymentMethod TEXT NOT NULL,
    PaidAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    RefundFlag INTEGER DEFAULT 0,
    RefundAmount NUMERIC DEFAULT 0,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);

-- =========================================
-- Inventory
-- =========================================

CREATE TABLE IF NOT EXISTS InventoryItems (
    InventoryItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    ItemName TEXT NOT NULL,
    QuantityOnHand NUMERIC DEFAULT 0
);

-- =========================================
-- Indexes
-- =========================================

CREATE INDEX IF NOT EXISTS idx_orders_table ON Orders(TableId);
CREATE INDEX IF NOT EXISTS idx_orders_status ON Orders(OrderStatusId);
CREATE INDEX IF NOT EXISTS idx_orderitems_order ON OrderItems(OrderId);
CREATE INDEX IF NOT EXISTS idx_payments_order ON Payments(OrderId);