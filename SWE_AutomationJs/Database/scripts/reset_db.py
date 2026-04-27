import sqlite3
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent
DB_PATH = BASE_DIR / "data" / "restaurant.db"
SQL_DIR = BASE_DIR / "sql"

SQL_FILES = [
    "001_schema.sql",
    "002_seed_lookup_data.sql",
    "003_seed_core_data.sql",
    "004_seed_orders.sql",
    "005_seed_payments.sql",
    "006_triggers_order_totals.sql",
    "007_views_payment_summary.sql",
    "008_triggers_payment_status.sql",
]


def main():
    DB_PATH.parent.mkdir(parents=True, exist_ok=True)

    if DB_PATH.exists():
        DB_PATH.unlink()
        print(f"Deleted old database: {DB_PATH}")

    conn = sqlite3.connect(DB_PATH)

    try:
        conn.execute("PRAGMA foreign_keys = ON;")

        for file_name in SQL_FILES:
            sql_path = SQL_DIR / file_name
            print(f"Running {file_name}...")

            if not sql_path.exists():
                raise FileNotFoundError(f"Missing SQL file: {sql_path}")

            sql = sql_path.read_text(encoding="utf-8")

            if not sql.strip():
                raise ValueError(f"SQL file is empty: {sql_path}")

            conn.executescript(sql)
            conn.commit()

        print("\nDatabase reset completed successfully.")
        print(f"Database path: {DB_PATH}")

    except Exception as e:
        conn.rollback()
        print(f"\nDatabase reset failed: {e}")
        raise

    finally:
        conn.close()


if __name__ == "__main__":
    main()