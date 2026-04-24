#!/bin/zsh

set -e

cd "$(dirname "$0")"

rm -f data/restaurant.db

python3 scripts/init_db.py
python3 scripts/run_sql_file.py sql/002_seed_lookup_data.sql
python3 scripts/run_sql_file.py sql/003_seed_core_data.sql
python3 scripts/run_sql_file.py sql/006_triggers_order_totals.sql
python3 scripts/run_sql_file.py sql/004_seed_orders.sql
python3 scripts/run_sql_file.py sql/007_views_payment_summary.sql
python3 scripts/run_sql_file.py sql/005_seed_payments.sql
python3 scripts/run_sql_file.py sql/008_triggers_payment_status.sql

python3 scripts/verify_db.py

sqlite3 data/restaurant.db "UPDATE OrderItems SET Qty = Qty;"

echo "Setup complete."