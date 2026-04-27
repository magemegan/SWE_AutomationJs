import sqlite3
import sys
import time
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent
DB_PATH = BASE_DIR / "data" / "restaurant.db"


def run_sql_file(sql_file_relative_path: str):
    sql_path = (BASE_DIR / sql_file_relative_path).resolve()

    print(f"Using database: {DB_PATH}")
    print(f"Running SQL file: {sql_path}")

    DB_PATH.parent.mkdir(parents=True, exist_ok=True)
    DB_PATH.touch(exist_ok=True)

    if not sql_path.exists():
        raise FileNotFoundError(f"SQL file not found: {sql_path}")

    with open(sql_path, "r", encoding="utf-8") as f:
        sql = f.read()

    if not sql.strip():
        raise ValueError(f"SQL file is empty: {sql_path}")

    print("First 200 characters of SQL:")
    print(sql[:200])
    print("-" * 50)

    conn = sqlite3.connect(DB_PATH)

    try:
        conn.execute("PRAGMA foreign_keys = ON;")
        start_time = time.time()

        conn.executescript(sql)
        conn.commit()

        elapsed = time.time() - start_time
        print(f"Executed successfully in {elapsed:.2f} seconds.")

    except Exception as e:
        conn.rollback()
        print(f"Error executing SQL file: {e}")
        raise

    finally:
        conn.close()


if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: python scripts/run_sql_file.py <relative_sql_path>")
        sys.exit(1)

    run_sql_file(sys.argv[1])