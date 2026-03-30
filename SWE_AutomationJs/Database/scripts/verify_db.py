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

    cursor.execute("""
        SELECT type, name
        FROM sqlite_master
        WHERE type IN ('table', 'view', 'trigger')
        ORDER BY type, name
    """)

    rows = cursor.fetchall()
    conn.close()

    if not rows:
        print("No tables, views, or triggers found.")
        return

    print("Database objects found:")
    for obj_type, name in rows:
        print(f"{obj_type}: {name}")

if __name__ == "__main__":
    verify_database()