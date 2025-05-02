#!/bin/bash

# Get the directory of this script
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"
ROOT_DIR="$( cd "$SCRIPT_DIR/.." &> /dev/null && pwd )"

# Load environment variables
source "$SCRIPT_DIR/load-env.sh"

# Run the query passed via stdin
psql "${DB_CONNECTION_STRING}" -q 