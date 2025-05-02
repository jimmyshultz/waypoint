#!/bin/bash

# Script to run the API locally for testing

# Determine script directory and project root
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

# Navigate to API project directory
cd "$ROOT_DIR/backend/Waypoint.Api"

# Check if .env file exists and extract environment variables
if [ -f "$ROOT_DIR/.env" ]; then
    echo "Loading environment variables from .env file..."
    
    # Extract connection string without the quotes
    DB_CONNECTION_STRING=$(grep DB_CONNECTION_STRING "$ROOT_DIR/.env" | cut -d '=' -f2- | sed 's/^"//' | sed 's/"$//')
    JWT_SECRET=$(grep JWT_SECRET "$ROOT_DIR/.env" | cut -d '=' -f2)
    JWT_ISSUER=$(grep JWT_ISSUER "$ROOT_DIR/.env" | cut -d '=' -f2)
    JWT_AUDIENCE=$(grep JWT_AUDIENCE "$ROOT_DIR/.env" | cut -d '=' -f2)
    JWT_EXPIRY_MINUTES=$(grep JWT_EXPIRY_MINUTES "$ROOT_DIR/.env" | cut -d '=' -f2)
    
    # Export variables
    export DB_CONNECTION_STRING
    export JWT_SECRET
    export JWT_ISSUER
    export JWT_AUDIENCE
    export JWT_EXPIRY_MINUTES
    
    echo "Connection string loaded."
else
    echo "Warning: .env file not found. Make sure you've set up your environment variables."
    echo "You can run the setup-local-dev.sh script to create an initial .env file."
fi

# Set fixed ports for the API - Note: The app defaults to port 5258 regardless of these settings
export ASPNETCORE_URLS="http://localhost:5259;https://localhost:5260"

echo "Starting API server..."
echo "API will be available at: http://localhost:5258"
echo "Test endpoints:"
echo " - GET: http://localhost:5258/api/test/db-connection - Tests database connectivity"
echo " - GET: http://localhost:5258/api/test/create-tables - Creates database tables"
echo " - GET: http://localhost:5258/api/test/seed-test-data - Creates test data"
echo " - GET: http://localhost:5258/api/test/hosts - Lists all hosts"
echo " - GET: http://localhost:5258/api/test/stays - Lists all stays"

# Run the API in development mode
dotnet run --environment Development 