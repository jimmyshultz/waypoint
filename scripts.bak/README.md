# Utility Scripts

This directory contains utility scripts for the Waypoint application.

## Available Scripts

| Script | Description |
|--------|-------------|
| `setup-dev.sh` | Initializes the development environment |
| `load-env.sh` | Loads environment variables from .env file |
| `apply-migrations.sh` | Applies database migrations |
| `revert-migrations.sh` | Reverts database migrations |
| `psql-query.sh` | Runs SQL queries against the database |

## Usage

### Environment Setup

```bash
# Initialize development environment
./scripts/setup-dev.sh
```

This will:
- Create a `.env` file from `.env.example` if it doesn't exist
- Make all scripts executable

### Database Operations

```bash
# Apply database migrations
./scripts/apply-migrations.sh

# Revert database migrations (caution: this will delete all data)
./scripts/revert-migrations.sh

# Run a SQL query
echo "SELECT * FROM \"Users\";" | ./scripts/psql-query.sh

# Run a SQL query from a file
./scripts/psql-query.sh < your_query.sql
```

## Adding New Scripts

When adding new scripts:

1. Make them executable: `chmod +x scripts/your_script.sh`
2. Follow the pattern of using `load-env.sh` to access environment variables
3. Add documentation to this README 