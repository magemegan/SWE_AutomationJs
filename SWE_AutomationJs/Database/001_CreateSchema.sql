/* =========================================
   Automation of J's - 001_CreateSchema.sql
   SQL Server / SSMS
   ========================================= */

IF DB_ID('AutomationOfJs') IS NULL
BEGIN
    CREATE DATABASE AutomationOfJs;
END;
GO

USE AutomationOfJs;
GO

/* =========================================
   Drop tables in dependency order (for reset)
   ========================================= */
IF OBJECT_ID('dbo.TimeClock', 'U') IS NOT NULL DROP TABLE dbo.TimeClock;
IF OBJECT_ID('dbo.OrderItems', 'U') IS NOT NULL DROP TABLE dbo.OrderItems;
IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID('dbo.OrderStatus', 'U') IS NOT NULL DROP TABLE dbo.OrderStatus;
IF OBJECT_ID('dbo.MenuItems', 'U') IS NOT NULL DROP TABLE dbo.MenuItems;
IF OBJECT_ID('dbo.MenuCategories', 'U') IS NOT NULL DROP TABLE dbo.MenuCategories;
IF OBJECT_ID('dbo.TableStatusEvents', 'U') IS NOT NULL DROP TABLE dbo.TableStatusEvents;
IF OBJECT_ID('dbo.TableStatus', 'U') IS NOT NULL DROP TABLE dbo.TableStatus;
IF OBJECT_ID('dbo.DiningTables', 'U') IS NOT NULL DROP TABLE dbo.DiningTables;
IF OBJECT_ID('dbo.Employees', 'U') IS NOT NULL DROP TABLE dbo.Employees;
IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL DROP TABLE dbo.Roles;
GO

/* =========================================
   Roles
   ========================================= */
CREATE TABLE dbo.Roles
(
    RoleId INT IDENTITY(1,1) NOT NULL,
    RoleName NVARCHAR(30) NOT NULL,
    CONSTRAINT PK_Roles PRIMARY KEY (RoleId),
    CONSTRAINT UQ_Roles_RoleName UNIQUE (RoleName)
);
GO

/* =========================================
   Employees
   ========================================= */
CREATE TABLE dbo.Employees
(
    EmployeeId CHAR(6) NOT NULL,
    RoleId INT NOT NULL,
    FullName NVARCHAR(80) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Employees_IsActive DEFAULT (1),
    CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_Employees_CreatedAt DEFAULT (SYSDATETIME()),

    CONSTRAINT PK_Employees PRIMARY KEY (EmployeeId),
    CONSTRAINT FK_Employees_Roles FOREIGN KEY (RoleId)
        REFERENCES dbo.Roles(RoleId),

    CONSTRAINT CK_Employees_EmployeeId_Length CHECK (LEN(EmployeeId) = 6)
);
GO

/* =========================================
   DiningTables
   ========================================= */
CREATE TABLE dbo.DiningTables
(
    TableId INT IDENTITY(1,1) NOT NULL,
    TableCode VARCHAR(2) NOT NULL,
    ColLetter CHAR(1) NOT NULL,
    RowNumber TINYINT NOT NULL,

    CONSTRAINT PK_DiningTables PRIMARY KEY (TableId),
    CONSTRAINT UQ_DiningTables_TableCode UNIQUE (TableCode),
    CONSTRAINT CK_DiningTables_ColLetter CHECK (ColLetter IN ('A','B','C','D','E','F')),
    CONSTRAINT CK_DiningTables_RowNumber CHECK (RowNumber BETWEEN 1 AND 6)
);
GO

/* =========================================
   TableStatus
   ========================================= */
CREATE TABLE dbo.TableStatus
(
    TableStatusId INT IDENTITY(1,1) NOT NULL,
    StatusName NVARCHAR(20) NOT NULL,

    CONSTRAINT PK_TableStatus PRIMARY KEY (TableStatusId),
    CONSTRAINT UQ_TableStatus_StatusName UNIQUE (StatusName)
);
GO

/* =========================================
   TableStatusEvents
   ========================================= */
CREATE TABLE dbo.TableStatusEvents
(
    TableStatusEventId BIGINT IDENTITY(1,1) NOT NULL,
    TableId INT NOT NULL,
    OldTableStatusId INT NULL,
    NewTableStatusId INT NOT NULL,
    ChangedByEmployeeId CHAR(6) NOT NULL,
    ChangedAt DATETIME2(0) NOT NULL CONSTRAINT DF_TableStatusEvents_ChangedAt DEFAULT (SYSDATETIME()),

    CONSTRAINT PK_TableStatusEvents PRIMARY KEY (TableStatusEventId),
    CONSTRAINT FK_TableStatusEvents_DiningTables FOREIGN KEY (TableId)
        REFERENCES dbo.DiningTables(TableId),
    CONSTRAINT FK_TableStatusEvents_OldStatus FOREIGN KEY (OldTableStatusId)
        REFERENCES dbo.TableStatus(TableStatusId),
    CONSTRAINT FK_TableStatusEvents_NewStatus FOREIGN KEY (NewTableStatusId)
        REFERENCES dbo.TableStatus(TableStatusId),
    CONSTRAINT FK_TableStatusEvents_Employees FOREIGN KEY (ChangedByEmployeeId)
        REFERENCES dbo.Employees(EmployeeId)
);
GO

/* =========================================
   MenuCategories
   ========================================= */
CREATE TABLE dbo.MenuCategories
(
    CategoryId INT IDENTITY(1,1) NOT NULL,
    CategoryName NVARCHAR(50) NOT NULL,

    CONSTRAINT PK_MenuCategories PRIMARY KEY (CategoryId),
    CONSTRAINT UQ_MenuCategories_CategoryName UNIQUE (CategoryName)
);
GO

/* =========================================
   MenuItems
   ========================================= */
CREATE TABLE dbo.MenuItems
(
    MenuItemId INT IDENTITY(1,1) NOT NULL,
    CategoryId INT NOT NULL,
    ItemName NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_MenuItems_IsActive DEFAULT (1),

    CONSTRAINT PK_MenuItems PRIMARY KEY (MenuItemId),
    CONSTRAINT FK_MenuItems_MenuCategories FOREIGN KEY (CategoryId)
        REFERENCES dbo.MenuCategories(CategoryId),
    CONSTRAINT UQ_MenuItems_Category_Item UNIQUE (CategoryId, ItemName),
    CONSTRAINT CK_MenuItems_Price CHECK (Price >= 0)
);
GO

/* =========================================
   OrderStatus
   ========================================= */
CREATE TABLE dbo.OrderStatus
(
    OrderStatusId INT IDENTITY(1,1) NOT NULL,
    StatusName NVARCHAR(20) NOT NULL,

    CONSTRAINT PK_OrderStatus PRIMARY KEY (OrderStatusId),
    CONSTRAINT UQ_OrderStatus_StatusName UNIQUE (StatusName)
);
GO

/* =========================================
   Orders
   ========================================= */
CREATE TABLE dbo.Orders
(
    OrderId BIGINT IDENTITY(1,1) NOT NULL,
    TableId INT NOT NULL,
    ServerEmployeeId CHAR(6) NOT NULL,
    OrderStatusId INT NOT NULL,
    OpenedAt DATETIME2(0) NOT NULL CONSTRAINT DF_Orders_OpenedAt DEFAULT (SYSDATETIME()),
    SubmittedAt DATETIME2(0) NULL,
    ReadyAt DATETIME2(0) NULL,
    ClosedAt DATETIME2(0) NULL,

    CONSTRAINT PK_Orders PRIMARY KEY (OrderId),
    CONSTRAINT FK_Orders_DiningTables FOREIGN KEY (TableId)
        REFERENCES dbo.DiningTables(TableId),
    CONSTRAINT FK_Orders_Employees FOREIGN KEY (ServerEmployeeId)
        REFERENCES dbo.Employees(EmployeeId),
    CONSTRAINT FK_Orders_OrderStatus FOREIGN KEY (OrderStatusId)
        REFERENCES dbo.OrderStatus(OrderStatusId)
);
GO

/* =========================================
   OrderItems
   ========================================= */
CREATE TABLE dbo.OrderItems
(
    OrderItemId BIGINT IDENTITY(1,1) NOT NULL,
    OrderId BIGINT NOT NULL,
    MenuItemId INT NOT NULL,
    SeatNumber TINYINT NOT NULL,
    Qty INT NOT NULL,
    UnitPriceAtSale DECIMAL(10,2) NOT NULL,
    Notes NVARCHAR(200) NULL,
    CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_OrderItems_CreatedAt DEFAULT (SYSDATETIME()),

    CONSTRAINT PK_OrderItems PRIMARY KEY (OrderItemId),
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId)
        REFERENCES dbo.Orders(OrderId),
    CONSTRAINT FK_OrderItems_MenuItems FOREIGN KEY (MenuItemId)
        REFERENCES dbo.MenuItems(MenuItemId),

    CONSTRAINT CK_OrderItems_SeatNumber CHECK (SeatNumber BETWEEN 1 AND 4),
    CONSTRAINT CK_OrderItems_Qty CHECK (Qty > 0),
    CONSTRAINT CK_OrderItems_UnitPrice CHECK (UnitPriceAtSale >= 0)
);
GO

/* =========================================
   TimeClock
   ========================================= */
CREATE TABLE dbo.TimeClock
(
    TimeClockId BIGINT IDENTITY(1,1) NOT NULL,
    EmployeeId CHAR(6) NOT NULL,
    ClockIn DATETIME2(0) NOT NULL,
    ClockOut DATETIME2(0) NULL,

    CONSTRAINT PK_TimeClock PRIMARY KEY (TimeClockId),
    CONSTRAINT FK_TimeClock_Employees FOREIGN KEY (EmployeeId)
        REFERENCES dbo.Employees(EmployeeId)
);
GO

/* =========================================
   Indexes
   ========================================= */
CREATE INDEX IX_Employees_RoleId
    ON dbo.Employees(RoleId);

CREATE INDEX IX_TableStatusEvents_TableId_ChangedAt
    ON dbo.TableStatusEvents(TableId, ChangedAt);

CREATE INDEX IX_Orders_Status_SubmittedAt
    ON dbo.Orders(OrderStatusId, SubmittedAt);

CREATE INDEX IX_Orders_TableId_OpenedAt
    ON dbo.Orders(TableId, OpenedAt);

CREATE INDEX IX_OrderItems_OrderId
    ON dbo.OrderItems(OrderId);

CREATE INDEX IX_TimeClock_EmployeeId_ClockIn
    ON dbo.TimeClock(EmployeeId, ClockIn);
GO