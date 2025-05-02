#!/bin/bash

# Get the directory of this script
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"
ROOT_DIR="$( cd "$SCRIPT_DIR/.." &> /dev/null && pwd )"

# Load environment variables
source "$SCRIPT_DIR/load-env.sh"

# Revert the initial schema migration
echo "Reverting migration: $ROOT_DIR/migrations/01_initial_schema_down.sql"
psql "${DB_CONNECTION_STRING}" -f "$ROOT_DIR/migrations/01_initial_schema_down.sql"

echo "Migration reverted successfully!"
echo "Database schema has been removed." 