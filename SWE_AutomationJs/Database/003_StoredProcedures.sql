/* =========================================
   Automation of J's - 003_StoredProcedures.sql
   SQL Server / SSMS
   ========================================= */

USE AutomationOfJs;
GO

/* =========================================
   Drop procedures if they already exist
   ========================================= */
IF OBJECT_ID('dbo.usp_Login', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Login;
IF OBJECT_ID('dbo.usp_OpenOrder', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_OpenOrder;
IF OBJECT_ID('dbo.usp_AddOrderItem', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_AddOrderItem;
IF OBJECT_ID('dbo.usp_SubmitOrder', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_SubmitOrder;
GO

/* =========================================
   usp_Login
   Validates employee login and returns role info
   NOTE: plain-text comparison for now
   ========================================= */
CREATE PROCEDURE dbo.usp_Login
    @EmployeeId CHAR(6),
    @Password   NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        e.EmployeeId,
        e.FullName,
        e.RoleId,
        r.RoleName,
        e.IsActive
    FROM dbo.Employees e
    INNER JOIN dbo.Roles r
        ON e.RoleId = r.RoleId
    WHERE e.EmployeeId = @EmployeeId
      AND e.PasswordHash = @Password
      AND e.IsActive = 1;
END;
GO

/* =========================================
   usp_OpenOrder
   Opens a new order for a table
   Business rules:
   - only waiter or manager can open orders
   - table must not already have an active order
   - new order starts in Open status
   ========================================= */
CREATE PROCEDURE dbo.usp_OpenOrder
    @TableId INT,
    @ServerEmployeeId CHAR(6)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RoleName NVARCHAR(30);
    DECLARE @OpenStatusId INT;
    DECLARE @ActiveOrderCount INT;

    SELECT @RoleName = r.RoleName
    FROM dbo.Employees e
    INNER JOIN dbo.Roles r
        ON e.RoleId = r.RoleId
    WHERE e.EmployeeId = @ServerEmployeeId
      AND e.IsActive = 1;

    IF @RoleName IS NULL
    BEGIN
        THROW 50001, 'Invalid or inactive employee.', 1;
    END;

    IF @RoleName NOT IN ('Waiter', 'Manager')
    BEGIN
        THROW 50002, 'Only a waiter or manager can open an order.', 1;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.DiningTables
        WHERE TableId = @TableId
    )
    BEGIN
        THROW 50003, 'Table does not exist.', 1;
    END;

    SELECT @OpenStatusId = OrderStatusId
    FROM dbo.OrderStatus
    WHERE StatusName = 'Open';

    SELECT @ActiveOrderCount = COUNT(*)
    FROM dbo.Orders o
    INNER JOIN dbo.OrderStatus os
        ON o.OrderStatusId = os.OrderStatusId
    WHERE o.TableId = @TableId
      AND os.StatusName NOT IN ('Closed', 'Cancelled');

    IF @ActiveOrderCount > 0
    BEGIN
        THROW 50004, 'Table already has an active order.', 1;
    END;

    INSERT INTO dbo.Orders
    (
        TableId,
        ServerEmployeeId,
        OrderStatusId
    )
    VALUES
    (
        @TableId,
        @ServerEmployeeId,
        @OpenStatusId
    );

    SELECT SCOPE_IDENTITY() AS NewOrderId;
END;
GO

/* =========================================
   usp_AddOrderItem
   Adds an item to an order
   Business rules:
   - only Open orders can be edited
   - seat number must be 1-4
   - qty must be > 0
   - unit price copied from MenuItems
   ========================================= */
CREATE PROCEDURE dbo.usp_AddOrderItem
    @OrderId BIGINT,
    @MenuItemId INT,
    @SeatNumber TINYINT,
    @Qty INT,
    @Notes NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StatusName NVARCHAR(20);
    DECLARE @UnitPrice DECIMAL(10,2);

    IF @SeatNumber NOT BETWEEN 1 AND 4
    BEGIN
        THROW 50005, 'SeatNumber must be between 1 and 4.', 1;
    END;

    IF @Qty <= 0
    BEGIN
        THROW 50006, 'Qty must be greater than 0.', 1;
    END;

    SELECT @StatusName = os.StatusName
    FROM dbo.Orders o
    INNER JOIN dbo.OrderStatus os
        ON o.OrderStatusId = os.OrderStatusId
    WHERE o.OrderId = @OrderId;

    IF @StatusName IS NULL
    BEGIN
        THROW 50007, 'Order does not exist.', 1;
    END;

    IF @StatusName <> 'Open'
    BEGIN
        THROW 50008, 'Only Open orders can be modified.', 1;
    END;

    SELECT @UnitPrice = Price
    FROM dbo.MenuItems
    WHERE MenuItemId = @MenuItemId
      AND IsActive = 1;

    IF @UnitPrice IS NULL
    BEGIN
        THROW 50009, 'Menu item does not exist or is inactive.', 1;
    END;

    INSERT INTO dbo.OrderItems
    (
        OrderId,
        MenuItemId,
        SeatNumber,
        Qty,
        UnitPriceAtSale,
        Notes
    )
    VALUES
    (
        @OrderId,
        @MenuItemId,
        @SeatNumber,
        @Qty,
        @UnitPrice,
        @Notes
    );

    SELECT SCOPE_IDENTITY() AS NewOrderItemId;
END;
GO

/* =========================================
   usp_SubmitOrder
   Submits an Open order to the kitchen
   Business rules:
   - only waiter or manager can submit
   - order must currently be Open
   - order must contain at least one item
   - set SubmittedAt timestamp
   ========================================= */
CREATE PROCEDURE dbo.usp_SubmitOrder
    @OrderId BIGINT,
    @EmployeeId CHAR(6)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RoleName NVARCHAR(30);
    DECLARE @CurrentStatus NVARCHAR(20);
    DECLARE @SubmittedStatusId INT;
    DECLARE @ItemCount INT;

    SELECT @RoleName = r.RoleName
    FROM dbo.Employees e
    INNER JOIN dbo.Roles r
        ON e.RoleId = r.RoleId
    WHERE e.EmployeeId = @EmployeeId
      AND e.IsActive = 1;

    IF @RoleName IS NULL
    BEGIN
        THROW 50010, 'Invalid or inactive employee.', 1;
    END;

    IF @RoleName NOT IN ('Waiter', 'Manager')
    BEGIN
        THROW 50011, 'Only a waiter or manager can submit an order.', 1;
    END;

    SELECT @CurrentStatus = os.StatusName
    FROM dbo.Orders o
    INNER JOIN dbo.OrderStatus os
        ON o.OrderStatusId = os.OrderStatusId
    WHERE o.OrderId = @OrderId;

    IF @CurrentStatus IS NULL
    BEGIN
        THROW 50012, 'Order does not exist.', 1;
    END;

    IF @CurrentStatus <> 'Open'
    BEGIN
        THROW 50013, 'Only Open orders can be submitted.', 1;
    END;

    SELECT @ItemCount = COUNT(*)
    FROM dbo.OrderItems
    WHERE OrderId = @OrderId;

    IF @ItemCount = 0
    BEGIN
        THROW 50014, 'Cannot submit an order with no items.', 1;
    END;

    SELECT @SubmittedStatusId = OrderStatusId
    FROM dbo.OrderStatus
    WHERE StatusName = 'Submitted';

    UPDATE dbo.Orders
    SET OrderStatusId = @SubmittedStatusId,
        SubmittedAt = SYSDATETIME()
    WHERE OrderId = @OrderId;

    SELECT
        o.OrderId,
        os.StatusName,
        o.SubmittedAt
    FROM dbo.Orders o
    INNER JOIN dbo.OrderStatus os
        ON o.OrderStatusId = os.OrderStatusId
    WHERE o.OrderId = @OrderId;
END;
GO