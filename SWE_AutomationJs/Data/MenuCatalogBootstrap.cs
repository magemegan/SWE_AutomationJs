using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace SWE_AutomationJs_UI_Design.Data
{
    internal static class MenuCatalogBootstrap
    {
        public static void EnsureConfiguredMenu()
        {
            using (var connection = Db.Open())
            {
                EnsureModifierSchema(connection);
                SeedCategories(connection);
                SeedMenuItems(connection);
                SeedOptionGroups(connection);
            }
        }

        private static void EnsureModifierSchema(Microsoft.Data.Sqlite.SqliteConnection connection)
        {
            connection.Execute(@"
CREATE TABLE IF NOT EXISTS MenuItemOptionGroups (
    OptionGroupId INTEGER PRIMARY KEY AUTOINCREMENT,
    MenuItemId INTEGER NOT NULL,
    GroupName TEXT NOT NULL,
    IsRequired INTEGER NOT NULL DEFAULT 0 CHECK (IsRequired IN (0,1)),
    MinSelections INTEGER NOT NULL DEFAULT 0 CHECK (MinSelections >= 0),
    MaxSelections INTEGER NOT NULL DEFAULT 1 CHECK (MaxSelections >= 1),
    DisplayOrder INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (MenuItemId) REFERENCES MenuItems(MenuItemId)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    UNIQUE (MenuItemId, GroupName)
);

CREATE TABLE IF NOT EXISTS MenuItemOptions (
    OptionId INTEGER PRIMARY KEY AUTOINCREMENT,
    OptionGroupId INTEGER NOT NULL,
    OptionName TEXT NOT NULL,
    PriceDelta NUMERIC NOT NULL DEFAULT 0.00,
    DisplayOrder INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (OptionGroupId) REFERENCES MenuItemOptionGroups(OptionGroupId)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    UNIQUE (OptionGroupId, OptionName)
);

CREATE TABLE IF NOT EXISTS OrderItemOptions (
    OrderItemOptionId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderItemId INTEGER NOT NULL,
    OptionGroupName TEXT NOT NULL,
    OptionName TEXT NOT NULL,
    PriceDelta NUMERIC NOT NULL DEFAULT 0.00,
    DisplayOrder INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (OrderItemId) REFERENCES OrderItems(OrderItemId)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);");
        }

        private static void SeedCategories(Microsoft.Data.Sqlite.SqliteConnection connection)
        {
            string[] categories =
            {
                "Appetizers",
                "Salads",
                "Entrees",
                "Sides",
                "Sandwiches",
                "Burgers",
                "Beverages"
            };

            foreach (string category in categories)
            {
                connection.Execute(
                    "INSERT OR IGNORE INTO MenuCategories (CategoryName) VALUES (@CategoryName);",
                    new { CategoryName = category });
            }
        }

        private static void SeedMenuItems(Microsoft.Data.Sqlite.SqliteConnection connection)
        {
            MenuSeedItem[] items =
            {
                new MenuSeedItem("Appetizers", "Chicken Nachos", 8.50m),
                new MenuSeedItem("Appetizers", "Pork Nachos", 8.50m),
                new MenuSeedItem("Appetizers", "Pork or Chicken Sliders", 5.00m),
                new MenuSeedItem("Appetizers", "Catfish Bites", 6.50m),
                new MenuSeedItem("Appetizers", "Fried Veggies", 6.50m),

                new MenuSeedItem("Salads", "House Salad", 7.50m),
                new MenuSeedItem("Salads", "Caesar Salad", 7.50m),
                new MenuSeedItem("Salads", "Sweet Potato Chicken Salad", 11.50m),

                new MenuSeedItem("Entrees", "Shrimp & Grits", 13.50m),
                new MenuSeedItem("Entrees", "Sweet Tea Fried Chicken", 11.50m),
                new MenuSeedItem("Entrees", "Caribbean Chicken", 11.50m),
                new MenuSeedItem("Entrees", "Grilled Pork Chops", 11.00m),
                new MenuSeedItem("Entrees", "New York Strip Steak", 17.00m),
                new MenuSeedItem("Entrees", "Seared Tuna", 15.00m),
                new MenuSeedItem("Entrees", "Captain Crunch Chicken Tenders", 11.50m),
                new MenuSeedItem("Entrees", "Shock Top Grouper Fingers", 11.50m),
                new MenuSeedItem("Entrees", "Mac & Cheese Bar", 8.50m),

                new MenuSeedItem("Sides", "Curly Fries", 2.50m),
                new MenuSeedItem("Sides", "Wing Chips", 2.50m),
                new MenuSeedItem("Sides", "Sweet Potato Fries", 2.50m),
                new MenuSeedItem("Sides", "Creamy Cabbage Slaw", 2.50m),
                new MenuSeedItem("Sides", "Adluh Cheese Grits", 2.50m),
                new MenuSeedItem("Sides", "Mashed Potatoes", 2.50m),
                new MenuSeedItem("Sides", "Mac & Cheese", 2.50m),
                new MenuSeedItem("Sides", "Seasonal Vegetables", 2.50m),
                new MenuSeedItem("Sides", "Baked Beans", 2.50m),

                new MenuSeedItem("Sandwiches", "Grilled Cheese", 5.50m),
                new MenuSeedItem("Sandwiches", "Chicken BLT&A", 10.00m),
                new MenuSeedItem("Sandwiches", "Philly", 13.50m),
                new MenuSeedItem("Sandwiches", "Club", 10.00m),
                new MenuSeedItem("Sandwiches", "Meatball Sub", 10.00m),

                new MenuSeedItem("Burgers", "Bacon Cheeseburger", 11.00m),
                new MenuSeedItem("Burgers", "Carolina Burger", 11.00m),
                new MenuSeedItem("Burgers", "Portobello Burger (V)", 8.50m),
                new MenuSeedItem("Burgers", "Vegan Boca Burger (V)", 10.50m),

                new MenuSeedItem("Beverages", "Sweet / Unsweetened Tea", 2.00m),
                new MenuSeedItem("Beverages", "Coke / Diet Coke", 2.00m),
                new MenuSeedItem("Beverages", "Sprite", 2.00m),
                new MenuSeedItem("Beverages", "Bottled Water", 2.00m),
                new MenuSeedItem("Beverages", "Lemonade", 2.00m),
                new MenuSeedItem("Beverages", "Orange Juice", 2.00m)
            };

            connection.Execute("UPDATE MenuItems SET IsActive = 0;");

            foreach (MenuSeedItem item in items)
            {
                connection.Execute(@"
INSERT OR IGNORE INTO MenuItems (CategoryId, ItemName, Price, IsActive)
VALUES (
    (SELECT CategoryId FROM MenuCategories WHERE CategoryName = @CategoryName),
    @ItemName,
    @Price,
    1
);",
                    item);

                connection.Execute(@"
UPDATE MenuItems
SET Price = @Price,
    IsActive = 1
WHERE ItemName = @ItemName
  AND CategoryId = (SELECT CategoryId FROM MenuCategories WHERE CategoryName = @CategoryName);",
                    item);
            }
        }

        private static void SeedOptionGroups(Microsoft.Data.Sqlite.SqliteConnection connection)
        {
            connection.Execute("DELETE FROM MenuItemOptions;");
            connection.Execute("DELETE FROM MenuItemOptionGroups;");

            string[] sideOptions =
            {
                "Curly Fries",
                "Wing Chips",
                "Sweet Potato Fries",
                "Creamy Cabbage Slaw",
                "Adluh Cheese Grits",
                "Mashed Potatoes",
                "Mac & Cheese",
                "Seasonal Vegetables",
                "Baked Beans"
            };

            AddOptionGroup(connection, "Appetizers", "Chicken Nachos", "Add Ons", false, 0, 1, 1,
                new OptionSeed("Add BBQ Sauce", 0.50m, 1));
            AddOptionGroup(connection, "Appetizers", "Pork Nachos", "Add Ons", false, 0, 1, 1,
                new OptionSeed("Add BBQ Sauce", 0.50m, 1));

            AddOptionGroup(connection, "Appetizers", "Pork or Chicken Sliders", "Protein", true, 1, 1, 1,
                new OptionSeed("Pork", 0m, 1),
                new OptionSeed("Chicken", 0m, 2));
            AddOptionGroup(connection, "Appetizers", "Pork or Chicken Sliders", "Sauce", true, 1, 1, 2,
                new OptionSeed("Chipotle", 0m, 1),
                new OptionSeed("Jim Beam BBQ", 0m, 2),
                new OptionSeed("Carolina Gold BBQ", 0m, 3));

            string[] entrees =
            {
                "Shrimp & Grits",
                "Sweet Tea Fried Chicken",
                "Caribbean Chicken",
                "Grilled Pork Chops",
                "New York Strip Steak",
                "Seared Tuna",
                "Captain Crunch Chicken Tenders",
                "Shock Top Grouper Fingers",
                "Mac & Cheese Bar"
            };

            foreach (string entree in entrees)
            {
                AddOptionGroup(connection, "Entrees", entree, "Choose First Side", true, 1, 1, 1, BuildOptions(sideOptions));
                AddOptionGroup(connection, "Entrees", entree, "Choose Second Side", true, 1, 1, 2, BuildOptions(sideOptions));
            }

            AddOptionGroup(connection, "Entrees", "Mac & Cheese Bar", "Add Toppings", false, 0, 12, 3,
                new OptionSeed("Pepper Jack Cheese", 0m, 1),
                new OptionSeed("Cheddar Cheese", 0m, 2),
                new OptionSeed("Swiss Cheese", 0m, 3),
                new OptionSeed("Mozzarella Cheese", 0m, 4),
                new OptionSeed("Goat Cheese", 0m, 5),
                new OptionSeed("Bacon", 0m, 6),
                new OptionSeed("Broccoli", 0m, 7),
                new OptionSeed("Mushrooms", 0m, 8),
                new OptionSeed("Grilled Onions", 0m, 9),
                new OptionSeed("Jalapenos", 0m, 10),
                new OptionSeed("Spinach", 0m, 11),
                new OptionSeed("Tomatoes", 0m, 12));

            AddOptionGroup(connection, "Beverages", "Sweet / Unsweetened Tea", "Tea Choice", true, 1, 1, 1,
                new OptionSeed("Sweet Tea", 0m, 1),
                new OptionSeed("Unsweetened Tea", 0m, 2));
            AddOptionGroup(connection, "Beverages", "Coke / Diet Coke", "Coke Choice", true, 1, 1, 1,
                new OptionSeed("Coke", 0m, 1),
                new OptionSeed("Diet Coke", 0m, 2));
        }

        private static OptionSeed[] BuildOptions(IEnumerable<string> options)
        {
            return options
                .Select((name, index) => new OptionSeed(name, 0m, index + 1))
                .ToArray();
        }

        private static void AddOptionGroup(
            Microsoft.Data.Sqlite.SqliteConnection connection,
            string categoryName,
            string itemName,
            string groupName,
            bool isRequired,
            int minSelections,
            int maxSelections,
            int displayOrder,
            params OptionSeed[] options)
        {
            int menuItemId = connection.ExecuteScalar<int>(@"
SELECT m.MenuItemId
FROM MenuItems m
INNER JOIN MenuCategories c ON c.CategoryId = m.CategoryId
WHERE c.CategoryName = @CategoryName
  AND m.ItemName = @ItemName;",
                new
                {
                    CategoryName = categoryName,
                    ItemName = itemName
                });

            connection.Execute(@"
INSERT INTO MenuItemOptionGroups
    (MenuItemId, GroupName, IsRequired, MinSelections, MaxSelections, DisplayOrder)
VALUES
    (@MenuItemId, @GroupName, @IsRequired, @MinSelections, @MaxSelections, @DisplayOrder);",
                new
                {
                    MenuItemId = menuItemId,
                    GroupName = groupName,
                    IsRequired = isRequired ? 1 : 0,
                    MinSelections = minSelections,
                    MaxSelections = maxSelections,
                    DisplayOrder = displayOrder
                });

            int optionGroupId = connection.ExecuteScalar<int>("SELECT last_insert_rowid();");
            foreach (OptionSeed option in options)
            {
                connection.Execute(@"
INSERT INTO MenuItemOptions
    (OptionGroupId, OptionName, PriceDelta, DisplayOrder)
VALUES
    (@OptionGroupId, @OptionName, @PriceDelta, @DisplayOrder);",
                    new
                    {
                        OptionGroupId = optionGroupId,
                        option.OptionName,
                        option.PriceDelta,
                        option.DisplayOrder
                    });
            }
        }

        private sealed class MenuSeedItem
        {
            public MenuSeedItem(string categoryName, string itemName, decimal price)
            {
                CategoryName = categoryName;
                ItemName = itemName;
                Price = price;
            }

            public string CategoryName { get; private set; }
            public string ItemName { get; private set; }
            public decimal Price { get; private set; }
        }

        private sealed class OptionSeed
        {
            public OptionSeed(string optionName, decimal priceDelta, int displayOrder)
            {
                OptionName = optionName;
                PriceDelta = priceDelta;
                DisplayOrder = displayOrder;
            }

            public string OptionName { get; private set; }
            public decimal PriceDelta { get; private set; }
            public int DisplayOrder { get; private set; }
        }
    }
}
