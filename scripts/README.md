# Development Scripts

This directory contains scripts for local development and testing.

## Available Scripts

| Script | Description |
|--------|-------------|
| `setup-local-dev.sh` | Sets up a local development environment |
| `run-api.sh` | Runs the API locally for testing |

## Usage

### Setup Local Development Environment

```bash
# Set up local development environment
./scripts/setup-local-dev.sh
```

This script will:
1. Check for required dependencies
2. Create a `.env` file from `.env.example` if it doesn't exist
3. Provide guidance on configuring your database

### Run API Locally

```bash
# Run the API locally
./scripts/run-api.sh
```

This script will:
1. Load environment variables from your `.env` file
2. Start the API on port 5258 (this is hardcoded in the application)
3. Display available test endpoints

#### API Test Endpoints

The API provides several test endpoints for verifying functionality:

- `GET /api/test/db-connection` - Tests database connectivity
- `GET /api/test/db-status` - Shows database status and table information
- `GET /api/test/create-tables` - Creates database tables
- `GET /api/test/seed-test-data` - Seeds test data
- `GET /api/test/hosts` - Lists all hosts
- `GET /api/test/stays` - Lists all stays

## GitHub Actions Workflows

Most database operations are now handled by GitHub Actions workflows. See `.github/workflows/README.md` for more information on using these workflows. 