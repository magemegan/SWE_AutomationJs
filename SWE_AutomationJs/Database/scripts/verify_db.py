import sqlite3
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent
DB_PATH = BASE_DIR / "data" / "restaurant.db"


def verify_database():
    print(f"Using database: {DB_PATH}")

    if not DB_PATH.exists():
        print("Database file does not exist.")
        return

    conn = sqlite3.connect(DB_PATH)
    cursor = conn.cursor()

    try:
        print("\nDatabase objects found:")
        cursor.execute("""
            SELECT type, name
            FROM sqlite_master
            WHERE type IN ('table', 'view', 'trigger')
            ORDER BY type, name
        """)
        objects = cursor.fetchall()

        if not objects:
            print("No tables, views, or triggers found.")
            return

        for obj_type, name in objects:
            if name.startswith("sqlite_"):
                continue
            print(f"- {obj_type}: {name}")

        print("\nRow counts:")
        tables_to_check = [
            "Roles",
            "Employees",
            "TableStatus",
            "DiningTables",
            "WaiterTableAssignments",
            "MenuCategories",
            "MenuItems",
            "OrderStatus",
            "Orders",
            "OrderItems",
            "Payments",
            "TimeClock",
            "TableStateEvents",
            "OrderStatusEvents",
            "InventoryItems",
        ]

        for table in tables_to_check:
            try:
                cursor.execute(f'SELECT COUNT(*) FROM "{table}"')
                count = cursor.fetchone()[0]
                print(f"- {table}: {count} rows")
            except sqlite3.Error as e:
                print(f"- {table}: ERROR ({e})")

        print("\nSample order summary:")
        try:
            cursor.execute("""
                SELECT
                    o.OrderId,
                    dt.TableCode,
                    e.FirstName || ' ' || e.LastName AS ServerName,
                    o.Subtotal,
                    o.Tax,
                    o.Total
                FROM Orders o
                JOIN DiningTables dt ON o.TableId = dt.TableId
                JOIN Employees e ON o.ServerEmployeeId = e.EmployeeId
                ORDER BY o.OrderId
                LIMIT 5
            """)
            rows = cursor.fetchall()

            if not rows:
                print("No orders found.")
            else:
                for row in rows:
                    print(
                        f"- Order {row[0]} | Table {row[1]} | Server: {row[2]} | "
                        f"Subtotal: {row[3]:.2f} | Tax: {row[4]:.2f} | Total: {row[5]:.2f}"
                    )
        except sqlite3.Error as e:
            print(f"Could not query sample order summary: {e}")

        print("\nPayment summary view check:")
        try:
            cursor.execute("""
                SELECT
                    OrderId,
                    OrderTotal,
                    TotalPaid,
                    BalanceRemaining,
                    PaymentStatus
                FROM vw_order_payment_summary
                ORDER BY OrderId
            """)
            rows = cursor.fetchall()

            if not rows:
                print("No rows found in vw_order_payment_summary.")
            else:
                for row in rows:
                    print(
                        f"- Order {row[0]} | Total: {row[1]:.2f} | Paid: {row[2]:.2f} | "
                        f"Balance: {row[3]:.2f} | Status: {row[4]}"
                    )
        except sqlite3.Error as e:
            print(f"Could not query payment summary view: {e}")

    finally:
        conn.close()


if __name__ == "__main__":
    verify_database()