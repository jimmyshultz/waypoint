#!/bin/bash

# Get the directory of this script
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"
ROOT_DIR="$( cd "$SCRIPT_DIR/.." &> /dev/null && pwd )"

# Check if .env file exists, if not create it from example
if [ ! -f "$ROOT_DIR/.env" ]; then
    echo "Creating .env file from .env.example"
    if [ -f "$ROOT_DIR/.env.example" ]; then
        cp "$ROOT_DIR/.env.example" "$ROOT_DIR/.env"
        echo "Please edit .env file and update with your credentials"
    else
        echo "Error: .env.example file not found"
        exit 1
    fi
fi

# Make sure scripts are executable
chmod +x "$SCRIPT_DIR/"*.sh

echo "Development environment setup complete!"
echo "Next steps:"
echo "1. Edit .env file with your credentials (if you haven't already)"
echo "2. Run './scripts/apply-migrations.sh' to apply database migrations" 