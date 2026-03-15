USE AutomationOfJs;
GO

-- 1. login
EXEC dbo.usp_Login @EmployeeId = 'WT0001', @Password = 'pass123';
GO

-- 2. open order for table A1
DECLARE @TableId INT;

SELECT @TableId = TableId
FROM dbo.DiningTables
WHERE TableCode = 'A1';

EXEC dbo.usp_OpenOrder
    @TableId = @TableId,
    @ServerEmployeeId = 'WT0001';
GO

-- 3. find newest order
DECLARE @OrderId BIGINT;

SELECT TOP 1 @OrderId = OrderId
FROM dbo.Orders
ORDER BY OrderId DESC;

-- 4. add item
EXEC dbo.usp_AddOrderItem
    @OrderId = @OrderId,
    @MenuItemId = 1,
    @SeatNumber = 1,
    @Qty = 1,
    @Notes = 'No onions';
GO

-- 5. submit order
EXEC dbo.usp_SubmitOrder
    @OrderId = @OrderId,
    @EmployeeId = 'WT0001';
GO

-- 6. verify
SELECT * FROM dbo.Orders;
SELECT * FROM dbo.OrderItems;
GO