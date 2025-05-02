# Database Migrations

This directory contains SQL migration scripts for the Waypoint application database.

## Using Neon PostgreSQL

The Waypoint application uses [Neon](https://neon.tech/) as its PostgreSQL database provider. Neon is a serverless PostgreSQL service with a generous free tier.

### Setup Steps

1. **Create a Neon account**:
   - Go to [Neon.tech](https://neon.tech/) and sign up for an account
   - Create a new project (e.g., "Waypoint")

2. **Create a new database**:
   - In your Neon project dashboard, create a new database named "waypoint"
   - Neon will provide you with a connection string

3. **Configure environment**:
   - Add the connection string to your `.env` file (see `.env.example` for format)

4. **Apply migrations**:
   - Run `./scripts/apply-migrations.sh` to create the schema

## Migration Files

- `01_initial_schema.sql` - Creates the initial database schema with Users, Hosts, and Stays tables
- `01_initial_schema_down.sql` - Reverts the initial schema (drops all tables)

## Running Migrations

Migrations are managed through scripts in the `/scripts` directory:

```bash
# Apply migrations
./scripts/apply-migrations.sh

# Revert migrations (caution: this will delete all data)
./scripts/revert-migrations.sh

# Execute specific SQL queries
echo "SELECT * FROM \"Users\";" | ./scripts/psql-query.sh
```

## Creating New Migrations

When adding new features that require database changes:

1. Create a new migration script with the next sequential number
2. Name it descriptively (e.g., `02_add_tour_tables.sql`)
3. Create a corresponding down migration (e.g., `02_add_tour_tables_down.sql`)
4. Update the version in the `DbVersionInfo` table in your migration

Example migration format:

```sql
-- 02_add_tour_tables.sql
-- Description: Adds tables for managing tours

-- Update version
INSERT INTO "DbVersionInfo" ("Version", "Description")
VALUES ('1.1.0', 'Add tour tables');

-- Add new tables
CREATE TABLE IF NOT EXISTS "Tours" (
    -- column definitions
);

-- Other schema changes
```

Down migration example:

```sql
-- 02_add_tour_tables_down.sql
-- Description: Reverts tour tables addition

-- Drop tables in reverse order
DROP TABLE IF EXISTS "Tours";

-- You don't need to remove the version entry
```

## Schema Overview

The database consists of these main tables:

- `Users` - Stores user account information
- `Hosts` - Stores host information with address and contact details
- `Stays` - Records historical stays with hosts
- `States` - Reference table of US state codes and names
- `DbVersionInfo` - Tracks database schema versions

For a more detailed schema description, see the main documentation at `docs/database.md`.

## Entity Relationships

- One User can have many Hosts (one-to-many)
- One Host can have many Stays (one-to-many)

## Backups

Neon provides automatic backups of your database. You can also:

1. Export your data manually using `pg_dump`:
   ```bash
   DB_CONNECTION_STRING=$(grep DB_CONNECTION_STRING .env | cut -d '=' -f2)
   pg_dump "${DB_CONNECTION_STRING}" > waypoint_backup_$(date +%Y%m%d).sql
   ```

2. Restore from a backup using `psql`:
   ```bash
   DB_CONNECTION_STRING=$(grep DB_CONNECTION_STRING .env | cut -d '=' -f2)
   psql "${DB_CONNECTION_STRING}" < waypoint_backup_20230101.sql
   ``` 