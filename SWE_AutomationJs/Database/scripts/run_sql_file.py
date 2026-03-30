import sqlite3
import sys
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent
DB_PATH = BASE_DIR / "data" / "restaurant.db"

def run_sql_file(sql_file_relative_path: str):
    sql_path = (BASE_DIR / sql_file_relative_path).resolve()

    print(f"Using database: {DB_PATH}")
    print(f"Running SQL file: {sql_path}")

    if not DB_PATH.exists():
        raise FileNotFoundError(f"Database file not found: {DB_PATH}")

    if not sql_path.exists():
        raise FileNotFoundError(f"SQL file not found: {sql_path}")

    with open(sql_path, "r", encoding="utf-8") as f:
        sql = f.read()

    print("First 200 characters:")
    print(sql[:200])

    if not sql.strip():
        raise ValueError(f"SQL file is empty: {sql_path}")

    conn = sqlite3.connect(DB_PATH)
    conn.execute("PRAGMA foreign_keys = ON;")
    conn.executescript(sql)
    conn.commit()
    conn.close()

    print("Executed successfully.")

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: python3 scripts/run_sql_file.py <relative_sql_path>")
        sys.exit(1)

    run_sql_file(sys.argv[1])