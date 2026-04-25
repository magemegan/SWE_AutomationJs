PRAGMA foreign_keys = ON;

-- =========================================
-- Lookup Tables
-- =========================================

CREATE TABLE Roles (
    RoleId INTEGER PRIMARY KEY AUTOINCREMENT,
    RoleName TEXT NOT NULL UNIQUE
);

CREATE TABLE TableStatus (
    TableStatusId INTEGER PRIMARY KEY AUTOINCREMENT,
    StatusName TEXT NOT NULL UNIQUE
);

CREATE TABLE OrderStatus (
    OrderStatusId INTEGER PRIMARY KEY AUTOINCREMENT,
    StatusName TEXT NOT NULL UNIQUE
);

CREATE TABLE MenuCategories (
    CategoryId INTEGER PRIMARY KEY AUTOINCREMENT,
    CategoryName TEXT NOT NULL UNIQUE
);

-- =========================================
-- Core Employee Tables
-- =========================================

CREATE TABLE Employees (
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

CREATE TABLE TimeClock (
    TimeClockId INTEGER PRIMARY KEY AUTOINCREMENT,
    EmployeeId TEXT NOT NULL,
    ClockIn DATETIME NOT NULL,
    ClockOut DATETIME,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CHECK (ClockOut IS NULL OR ClockOut >= ClockIn)
);

-- =========================================
-- Dining Room / Table Management
-- =========================================

CREATE TABLE DiningTables (
    TableId INTEGER PRIMARY KEY AUTOINCREMENT,
    TableCode TEXT NOT NULL UNIQUE,
    ColumnLetter TEXT NOT NULL,
    RowNumber INTEGER NOT NULL,
    CurrentTableStatusId INTEGER NOT NULL,
    SeatCapacity INTEGER NOT NULL CHECK (SeatCapacity >= 4),
    FOREIGN KEY (CurrentTableStatusId) REFERENCES TableStatus(TableStatusId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CHECK (length(ColumnLetter) = 1),
    CHECK (RowNumber BETWEEN 1 AND 6),
    CHECK (ColumnLetter IN ('A','B','C','D','E','F')),
    UNIQUE (ColumnLetter, RowNumber)
);

CREATE TABLE TableStateEvents (
    TableStatusEventId INTEGER PRIMARY KEY AUTOINCREMENT,
    TableId INTEGER NOT NULL,
    OldTableStatusId INTEGER,
    NewTableStatusId INTEGER NOT NULL,
    ChangedByEmployeeId TEXT NOT NULL,
    ChangedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (TableId) REFERENCES DiningTables(TableId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    FOREIGN KEY (OldTableStatusId) REFERENCES TableStatus(TableStatusId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    FOREIGN KEY (NewTableStatusId) REFERENCES TableStatus(TableStatusId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    FOREIGN KEY (ChangedByEmployeeId) REFERENCES Employees(EmployeeId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE WaiterTableAssignments (
    AssignmentId INTEGER PRIMARY KEY AUTOINCREMENT,
    EmployeeId TEXT NOT NULL,
    TableId INTEGER NOT NULL,
    AssignedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UnassignedAt DATETIME,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    FOREIGN KEY (TableId) REFERENCES DiningTables(TableId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CHECK (UnassignedAt IS NULL OR UnassignedAt >= AssignedAt),
    UNIQUE (EmployeeId, TableId, UnassignedAt)
);

-- =========================================
-- Menu
-- =========================================

CREATE TABLE MenuItems (
    MenuItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    CategoryId INTEGER NOT NULL,
    ItemName TEXT NOT NULL,
    Price NUMERIC NOT NULL CHECK (Price >= 0),
    IsActive INTEGER NOT NULL DEFAULT 1 CHECK (IsActive IN (0,1)),
    FOREIGN KEY (CategoryId) REFERENCES MenuCategories(CategoryId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    UNIQUE (CategoryId, ItemName)
);

-- =========================================
-- Orders
-- =========================================

CREATE TABLE Orders (
    OrderId INTEGER PRIMARY KEY AUTOINCREMENT,
    TableId INTEGER NOT NULL,
    ServerEmployeeId TEXT NOT NULL,
    OrderStatusId INTEGER NOT NULL,
    PartySize INTEGER CHECK (PartySize BETWEEN 1 AND 4),
    OpenedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    SubmittedAt DATETIME,
    ReadyAt DATETIME,
    DeliveredAt DATETIME,
    ClosedAt DATETIME,
    Subtotal NUMERIC NOT NULL DEFAULT 0.00 CHECK (Subtotal >= 0),
    Tax NUMERIC NOT NULL DEFAULT 0.00 CHECK (Tax >= 0),
    Total NUMERIC NOT NULL DEFAULT 0.00 CHECK (Total >= 0),
    FOREIGN KEY (TableId) REFERENCES DiningTables(TableId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    FOREIGN KEY (ServerEmployeeId) REFERENCES Employees(EmployeeId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    FOREIGN KEY (OrderStatusId) REFERENCES OrderStatus(OrderStatusId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CHECK (SubmittedAt IS NULL OR SubmittedAt >= OpenedAt),
    CHECK (ReadyAt IS NULL OR ReadyAt >= OpenedAt),
    CHECK (DeliveredAt IS NULL OR DeliveredAt >= OpenedAt),
    CHECK (ClosedAt IS NULL OR ClosedAt >= OpenedAt)
);

CREATE TABLE OrderStatusEvents (
    OrderStatusEventId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    OldOrderStatusId INTEGER,
    NewOrderStatusId INTEGER NOT NULL,
    ChangedByEmployeeId TEXT NOT NULL,
    ChangedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    FOREIGN KEY (OldOrderStatusId) REFERENCES OrderStatus(OrderStatusId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    FOREIGN KEY (NewOrderStatusId) REFERENCES OrderStatus(OrderStatusId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    FOREIGN KEY (ChangedByEmployeeId) REFERENCES Employees(EmployeeId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE OrderItems (
    OrderItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    MenuItemId INTEGER NOT NULL,
    SeatNumber INTEGER CHECK (SeatNumber BETWEEN 1 AND 4),
    Qty INTEGER NOT NULL CHECK (Qty > 0),
    UnitPriceAtSale NUMERIC NOT NULL CHECK (UnitPriceAtSale >= 0),
    Notes TEXT,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    FOREIGN KEY (MenuItemId) REFERENCES MenuItems(MenuItemId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

-- =========================================
-- Payments
-- =========================================

CREATE TABLE Payments (
    PaymentId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    Amount NUMERIC NOT NULL CHECK (Amount >= 0),
    PaymentMethod TEXT NOT NULL CHECK (PaymentMethod IN ('Cash', 'Card')),
    PaidAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ApprovedByEmployeeId TEXT,
    RefundFlag INTEGER NOT NULL DEFAULT 0 CHECK (RefundFlag IN (0,1)),
    RefundAmount NUMERIC NOT NULL DEFAULT 0.00 CHECK (RefundAmount >= 0),
    RefundApprovedByEmployeeId TEXT,
    RefundApprovedAt DATETIME,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    FOREIGN KEY (ApprovedByEmployeeId) REFERENCES Employees(EmployeeId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    FOREIGN KEY (RefundApprovedByEmployeeId) REFERENCES Employees(EmployeeId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CHECK (
        (RefundFlag = 0
         AND RefundAmount = 0.00
         AND RefundApprovedByEmployeeId IS NULL
         AND RefundApprovedAt IS NULL)
        OR
        (RefundFlag = 1
         AND RefundAmount >= 0
         AND RefundApprovedByEmployeeId IS NOT NULL
         AND RefundApprovedAt IS NOT NULL)
    )
);

-- =========================================
-- Optional Inventory Support
-- =========================================

CREATE TABLE InventoryItems (
    InventoryItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    ItemName TEXT NOT NULL UNIQUE,
    UnitOfMeasure TEXT NOT NULL,
    QuantityOnHand NUMERIC NOT NULL DEFAULT 0 CHECK (QuantityOnHand >= 0),
    ReorderLevel NUMERIC NOT NULL DEFAULT 0 CHECK (ReorderLevel >= 0),
    IsActive INTEGER NOT NULL DEFAULT 1 CHECK (IsActive IN (0,1))
);

-- =========================================
-- Indexes
-- =========================================

CREATE INDEX idx_employees_roleid
    ON Employees(RoleId);

CREATE INDEX idx_timeclock_employeeid
    ON TimeClock(EmployeeId);

CREATE INDEX idx_diningtables_status
    ON DiningTables(CurrentTableStatusId);

CREATE INDEX idx_diningtables_location
    ON DiningTables(ColumnLetter, RowNumber);

CREATE INDEX idx_tablestateevents_tableid
    ON TableStateEvents(TableId);

CREATE INDEX idx_tablestateevents_changedat
    ON TableStateEvents(ChangedAt);

CREATE INDEX idx_waiter_assignments_employeeid
    ON WaiterTableAssignments(EmployeeId);

CREATE INDEX idx_waiter_assignments_tableid
    ON WaiterTableAssignments(TableId);

CREATE INDEX idx_orders_tableid
    ON Orders(TableId);

CREATE INDEX idx_orders_serveremployeeid
    ON Orders(ServerEmployeeId);

CREATE INDEX idx_orders_statusid
    ON Orders(OrderStatusId);

CREATE INDEX idx_orderstatusevents_orderid
    ON OrderStatusEvents(OrderId);

CREATE INDEX idx_orderitems_orderid
    ON OrderItems(OrderId);

CREATE INDEX idx_orderitems_menuitemid
    ON OrderItems(MenuItemId);

CREATE INDEX idx_payments_orderid
    ON Payments(OrderId);

CREATE INDEX idx_inventoryitems_name
    ON InventoryItems(ItemName);
