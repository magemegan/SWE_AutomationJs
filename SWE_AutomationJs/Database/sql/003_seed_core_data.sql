PRAGMA foreign_keys = ON;

-- =========================================
-- Employees
-- =========================================

INSERT OR IGNORE INTO Employees
(EmployeeId, RoleId, FirstName, LastName, PasswordHash, IsActive)
VALUES
    ('E00001', 1, 'Tiffany', 'Brandon', 'hashed_admin_pw', 1),
    ('E00002', 2, 'Jordan', 'Lee', 'hashed_manager_pw', 1),
    ('E00003', 3, 'Ava', 'Smith', 'hashed_server_pw', 1),
    ('E00004', 4, 'Noah', 'Davis', 'hashed_cashier_pw', 1),
    ('E00005', 5, 'Mia', 'Brown', 'hashed_kitchen_pw', 1);

-- =========================================
-- Dining Tables (Fixed Layout)
-- =========================================

INSERT OR IGNORE INTO DiningTables
(TableCode, ColumnLetter, RowNumber, CurrentTableStatusId, SeatCapacity)
VALUES
    ('A1', 'A', 1, 1, 4),
    ('A2', 'A', 2, 1, 4),
    ('B1', 'B', 1, 1, 2),
    ('B2', 'B', 2, 2, 4),
    ('C1', 'C', 1, 3, 4);

-- =========================================
-- Waiter Table Assignments (NEW)
-- =========================================

INSERT INTO WaiterTableAssignments
(EmployeeId, TableId)
VALUES
    ('E00003', 1),
    ('E00003', 2),
    ('E00003', 3);

-- =========================================
-- Menu Items
-- =========================================

INSERT OR IGNORE INTO MenuItems
(CategoryId, ItemName, Price, IsActive)
VALUES
    (1, 'Mozzarella Sticks', 7.99, 1),
    (1, 'Loaded Fries', 8.49, 1),
    (2, 'Grilled Chicken', 14.99, 1),
    (2, 'Classic Burger', 12.49, 1),
    (2, 'Shrimp Pasta', 16.99, 1),
    (3, 'Cheesecake', 6.50, 1),
    (3, 'Chocolate Cake', 6.75, 1),
    (4, 'Sweet Tea', 2.99, 1),
    (4, 'Lemonade', 2.99, 1),
    (4, 'Coffee', 2.49, 1);

-- =========================================
-- Time Clock
-- =========================================

INSERT INTO TimeClock
(EmployeeId, ClockIn, ClockOut)
VALUES
    ('E00003', '2026-03-30 08:00:00', '2026-03-30 16:00:00'),
    ('E00004', '2026-03-30 09:00:00', '2026-03-30 17:00:00'),
    ('E00005', '2026-03-30 07:30:00', NULL);