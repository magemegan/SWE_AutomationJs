import sqlite3
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent
DB_PATH = BASE_DIR / "data" / "restaurant.db"
SCHEMA_PATH = BASE_DIR / "sql" / "001_schema.sql"


def initialize_database():
    DB_PATH.parent.mkdir(parents=True, exist_ok=True)

    conn = sqlite3.connect(DB_PATH)
    cursor = conn.cursor()

    try:
        cursor.execute("PRAGMA foreign_keys = OFF;")

        # Drop all existing user-defined tables, views, and triggers
        cursor.execute("""
            SELECT type, name
            FROM sqlite_master
            WHERE type IN ('table', 'view', 'trigger')
            ORDER BY type, name
        """)
        objects = cursor.fetchall()

        for obj_type, name in objects:
            if name.startswith("sqlite_"):
                continue

            if obj_type == "table":
                cursor.execute(f'DROP TABLE IF EXISTS "{name}"')
            elif obj_type == "view":
                cursor.execute(f'DROP VIEW IF EXISTS "{name}"')
            elif obj_type == "trigger":
                cursor.execute(f'DROP TRIGGER IF EXISTS "{name}"')

        conn.commit()

        if not SCHEMA_PATH.exists():
            raise FileNotFoundError(f"Schema file not found: {SCHEMA_PATH}")

        with open(SCHEMA_PATH, "r", encoding="utf-8") as f:
            schema_sql = f.read()

        if not schema_sql.strip():
            raise ValueError(f"Schema file is empty: {SCHEMA_PATH}")

        conn.executescript(schema_sql)
        conn.commit()

        cursor.execute("PRAGMA foreign_keys = ON;")

        cursor.execute("""
            SELECT type, name
            FROM sqlite_master
            WHERE type IN ('table', 'view', 'trigger')
            ORDER BY type, name
        """)
        created_objects = cursor.fetchall()

        print(f"Initialized database: {DB_PATH}")
        print("Database objects created:")
        for obj_type, name in created_objects:
            if name.startswith("sqlite_"):
                continue
            print(f"- {obj_type}: {name}")

    finally:
        conn.close()


if __name__ == "__main__":
    initialize_database()