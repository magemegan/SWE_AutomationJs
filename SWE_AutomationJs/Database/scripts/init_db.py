import sqlite3
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent
DB_PATH = BASE_DIR / "data" / "restaurant.db"
SCHEMA_PATH = BASE_DIR / "sql" / "001_schema.sql"

TABLES = [
    "Payments",
    "OrderItems",
    "Orders",
    "TableStateEvents",
    "TimeClock",
    "MenuItems",
    "MenuCategories",
    "DiningTables",
    "TableStatus",
    "OrderStatus",
    "Employees",
    "Roles",
]

def initialize_database():
    DB_PATH.parent.mkdir(parents=True, exist_ok=True)

    conn = sqlite3.connect(DB_PATH)
    conn.execute("PRAGMA foreign_keys = OFF;")
    cursor = conn.cursor()

    for table in TABLES:
        cursor.execute(f"DROP TABLE IF EXISTS {table}")

    conn.commit()

    with open(SCHEMA_PATH, "r", encoding="utf-8") as f:
        schema_sql = f.read()

    conn.executescript(schema_sql)
    conn.commit()

    cursor.execute("""
        SELECT name
        FROM sqlite_master
        WHERE type='table'
        ORDER BY name
    """)
    tables_created = [row[0] for row in cursor.fetchall()]

    conn.close()

    print(f"Initialized database: {DB_PATH}")
    print("Tables created:")
    for name in tables_created:
        print(f"- {name}")

if __name__ == "__main__":
    initialize_database()