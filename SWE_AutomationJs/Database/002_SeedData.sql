/* =========================================
   Automation of J's - 002_SeedData.sql
   SQL Server / SSMS
   ========================================= */

USE AutomationOfJs;
GO

/* =========================================
   Seed Roles
   ========================================= */
INSERT INTO dbo.Roles (RoleName)
VALUES
    ('Waiter'),
    ('Cook'),
    ('Busboy'),
    ('Manager');
GO

/* =========================================
   Seed TableStatus
   ========================================= */
INSERT INTO dbo.TableStatus (StatusName)
VALUES
    ('Ready'),
    ('Occupied'),
    ('Dirty');
GO

/* =========================================
   Seed OrderStatus
   ========================================= */
INSERT INTO dbo.OrderStatus (StatusName)
VALUES
    ('Open'),
    ('Submitted'),
    ('Preparing'),
    ('Ready'),
    ('Served'),
    ('Closed'),
    ('Cancelled');
GO

/* =========================================
   Seed DiningTables A1-F6
   ========================================= */
INSERT INTO dbo.DiningTables (TableCode, ColLetter, RowNumber)
VALUES
('A1','A',1), ('A2','A',2), ('A3','A',3), ('A4','A',4), ('A5','A',5), ('A6','A',6),
('B1','B',1), ('B2','B',2), ('B3','B',3), ('B4','B',4), ('B5','B',5), ('B6','B',6),
('C1','C',1), ('C2','C',2), ('C3','C',3), ('C4','C',4), ('C5','C',5), ('C6','C',6),
('D1','D',1), ('D2','D',2), ('D3','D',3), ('D4','D',4), ('D5','D',5), ('D6','D',6),
('E1','E',1), ('E2','E',2), ('E3','E',3), ('E4','E',4), ('E5','E',5), ('E6','E',6),
('F1','F',1), ('F2','F',2), ('F3','F',3), ('F4','F',4), ('F5','F',5), ('F6','F',6);
GO

/* =========================================
   Seed Employees (starter test accounts)
   NOTE: PasswordHash is placeholder text for now
   ========================================= */
INSERT INTO dbo.Employees (EmployeeId, RoleId, FullName, PasswordHash, IsActive)
VALUES
('WT0001', 1, 'Waiter One',  'pass123', 1),
('WT0002', 1, 'Waiter Two',  'pass123', 1),
('CK0001', 2, 'Cook One',    'pass123', 1),
('CK0002', 2, 'Cook Two',    'pass123', 1),
('BS0001', 3, 'Busboy One',  'pass123', 1),
('MG0001', 4, 'Manager One', 'pass123', 1);
GO

/* =========================================
   Seed MenuCategories
   ========================================= */
INSERT INTO dbo.MenuCategories (CategoryName)
VALUES
    ('Appetizers'),
    ('Salads'),
    ('Entrees'),
    ('Sides'),
    ('Sandwiches'),
    ('Burgers'),
    ('Beverages');
GO

/* =========================================
   Seed sample MenuItems
   Extend this to full menu later
   ========================================= */
INSERT INTO dbo.MenuItems (CategoryId, ItemName, Price, IsActive)
VALUES
    (1, 'Chicken Nachos', 8.50, 1),
    (1, 'Pork Nachos', 8.50, 1),
    (1, 'Catfish Bites', 6.50, 1),

    (2, 'House Salad', 7.50, 1),
    (2, 'Caesar Salad', 7.50, 1),

    (3, 'Shrimp & Grits', 13.50, 1),
    (3, 'Sweet Tea Fried Chicken', 11.50, 1),
    (3, 'New York Strip Steak', 17.00, 1),

    (4, 'Curly Fries', 2.50, 1),
    (4, 'Mac & Cheese', 2.50, 1),

    (5, 'Grilled Cheese', 5.50, 1),
    (5, 'Club', 10.00, 1),

    (6, 'Bacon Cheeseburger', 11.00, 1),
    (6, 'Carolina Burger', 11.00, 1),

    (7, 'Sweet Tea', 2.00, 1),
    (7, 'Coke', 2.00, 1),
    (7, 'Lemonade', 2.00, 1);
GO

/* =========================================
   Verification queries
   ========================================= */
SELECT * FROM dbo.Roles;
SELECT * FROM dbo.TableStatus;
SELECT * FROM dbo.OrderStatus;
SELECT * FROM dbo.DiningTables;
SELECT * FROM dbo.Employees;
SELECT * FROM dbo.MenuCategories;
SELECT * FROM dbo.MenuItems;
GO