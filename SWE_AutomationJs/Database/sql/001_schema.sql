PRAGMA foreign_keys = ON;

CREATE TABLE Roles (
    RoleId INTEGER PRIMARY KEY AUTOINCREMENT,
    RoleName TEXT NOT NULL UNIQUE
);

CREATE TABLE Employees (
    EmployeeId TEXT PRIMARY KEY,
    RoleId INTEGER NOT NULL,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    PasswordHash TEXT NOT NULL,
    IsActive INTEGER NOT NULL DEFAULT 1 CHECK (IsActive IN (0,1)),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

CREATE TABLE TableStatus (
    TableStatusId INTEGER PRIMARY KEY AUTOINCREMENT,
    StatusName TEXT NOT NULL UNIQUE
);

CREATE TABLE DiningTables (
    TableId INTEGER PRIMARY KEY AUTOINCREMENT,
    TableCode TEXT NOT NULL UNIQUE,
    CurrentTableStatusId INTEGER NOT NULL,
    SeatCapacity INTEGER NOT NULL CHECK (SeatCapacity > 0),
    RowNumber INTEGER,
    FOREIGN KEY (CurrentTableStatusId) REFERENCES TableStatus(TableStatusId)
);

CREATE TABLE TableStateEvents (
    TableStatusEventId INTEGER PRIMARY KEY AUTOINCREMENT,
    TableId INTEGER NOT NULL,
    OldTableStatusId INTEGER,
    NewTableStatusId INTEGER NOT NULL,
    ChangedByEmployeeId TEXT NOT NULL,
    ChangedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (TableId) REFERENCES DiningTables(TableId),
    FOREIGN KEY (OldTableStatusId) REFERENCES TableStatus(TableStatusId),
    FOREIGN KEY (NewTableStatusId) REFERENCES TableStatus(TableStatusId),
    FOREIGN KEY (ChangedByEmployeeId) REFERENCES Employees(EmployeeId)
);

CREATE TABLE OrderStatus (
    OrderStatusId INTEGER PRIMARY KEY AUTOINCREMENT,
    StatusName TEXT NOT NULL UNIQUE
);

CREATE TABLE Orders (
    OrderId INTEGER PRIMARY KEY AUTOINCREMENT,
    TableId INTEGER NOT NULL,
    ServerEmployeeId TEXT NOT NULL,
    OrderStatusId INTEGER NOT NULL,
    OpenedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    SubmittedAt DATETIME,
    ReadyAt DATETIME,
    ClosedAt DATETIME,
    Subtotal NUMERIC NOT NULL DEFAULT 0.00 CHECK (Subtotal >= 0),
    Tax NUMERIC NOT NULL DEFAULT 0.00 CHECK (Tax >= 0),
    Total NUMERIC NOT NULL DEFAULT 0.00 CHECK (Total >= 0),
    FOREIGN KEY (TableId) REFERENCES DiningTables(TableId),
    FOREIGN KEY (ServerEmployeeId) REFERENCES Employees(EmployeeId),
    FOREIGN KEY (OrderStatusId) REFERENCES OrderStatus(OrderStatusId)
);

CREATE TABLE MenuCategories (
    CategoryId INTEGER PRIMARY KEY AUTOINCREMENT,
    CategoryName TEXT NOT NULL UNIQUE
);

CREATE TABLE MenuItems (
    MenuItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    CategoryId INTEGER NOT NULL,
    ItemName TEXT NOT NULL,
    Price NUMERIC NOT NULL CHECK (Price >= 0),
    IsActive INTEGER NOT NULL DEFAULT 1 CHECK (IsActive IN (0,1)),
    FOREIGN KEY (CategoryId) REFERENCES MenuCategories(CategoryId)
);

CREATE TABLE OrderItems (
    OrderItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    MenuItemId INTEGER NOT NULL,
    SeatNumber INTEGER,
    Qty INTEGER NOT NULL CHECK (Qty > 0),
    UnitPriceAtSale NUMERIC NOT NULL CHECK (UnitPriceAtSale >= 0),
    Notes TEXT,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (MenuItemId) REFERENCES MenuItems(MenuItemId)
);

CREATE TABLE Payments (
    PaymentId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    Amount NUMERIC NOT NULL CHECK (Amount >= 0),
    PaymentMethod TEXT NOT NULL,
    PaidAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ApprovedByEmployeeId TEXT,
    RefundFlag INTEGER NOT NULL DEFAULT 0 CHECK (RefundFlag IN (0,1)),
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ApprovedByEmployeeId) REFERENCES Employees(EmployeeId)
);

CREATE TABLE TimeClock (
    TimeClockId INTEGER PRIMARY KEY AUTOINCREMENT,
    EmployeeId TEXT NOT NULL,
    ClockIn DATETIME NOT NULL,
    ClockOut DATETIME,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
);

CREATE INDEX idx_employees_roleid ON Employees(RoleId);
CREATE INDEX idx_diningtables_status ON DiningTables(CurrentTableStatusId);
CREATE INDEX idx_tablestateevents_tableid ON TableStateEvents(TableId);
CREATE INDEX idx_tablestateevents_changedat ON TableStateEvents(ChangedAt);
CREATE INDEX idx_orders_tableid ON Orders(TableId);
CREATE INDEX idx_orders_serveremployeeid ON Orders(ServerEmployeeId);
CREATE INDEX idx_orders_statusid ON Orders(OrderStatusId);
CREATE INDEX idx_orderitems_orderid ON OrderItems(OrderId);
CREATE INDEX idx_orderitems_menuitemid ON OrderItems(MenuItemId);
CREATE INDEX idx_payments_orderid ON Payments(OrderId);
CREATE INDEX idx_timeclock_employeeid ON TimeClock(EmployeeId);