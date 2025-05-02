#!/bin/bash

# Get the directory of this script
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"
# Get the root directory of the project
ROOT_DIR="$( cd "$SCRIPT_DIR/.." &> /dev/null && pwd )"
ENV_FILE="$ROOT_DIR/.env"

# Check if .env file exists
if [ ! -f "$ENV_FILE" ]; then
    echo "Error: .env file not found at $ENV_FILE"
    echo "Please copy .env.example to .env and update with your credentials"
    exit 1
fi

# Load environment variables from .env file
export $(grep -v '^#' "$ENV_FILE" | xargs)

echo "Environment variables loaded from $ENV_FILE" 