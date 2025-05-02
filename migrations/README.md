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
   - Use the GitHub Actions "Apply Migrations" workflow to create the schema
   - Or run the API and use the test endpoint (`/api/test/create-tables`)

## Migration Files

- `01_initial_schema.sql` - Creates the initial database schema with Users, Hosts, and Stays tables
- `01_initial_schema_down.sql` - Reverts the initial schema (drops all tables)

## Running Migrations

Migrations are managed through GitHub Actions workflows:

```bash
# Apply migrations (via GitHub Actions)
1. Go to Actions tab in your repository
2. Select "Apply Migrations" workflow
3. Click "Run workflow"

# Alternatively, use the API test endpoint:
curl http://localhost:5258/api/test/create-tables
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
INSERT INTO "DbVersionInfo" ("Version", "Description", "AppliedAt")
VALUES ('1.1.0', 'Add tour tables', NOW());

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

## The DbVersionInfo Table

The `DbVersionInfo` table tracks database migrations and schema versions:

```sql
CREATE TABLE IF NOT EXISTS "DbVersionInfo" (
    "Id" SERIAL PRIMARY KEY,
    "Version" VARCHAR(20) NOT NULL,
    "Description" VARCHAR(255) NOT NULL,
    "AppliedAt" TIMESTAMP NOT NULL DEFAULT NOW()
);
```

Each migration creates an entry in this table with:
- `Version`: The schema version (e.g., "1.0.0")
- `Description`: A brief description of the migration
- `AppliedAt`: When the migration was applied

This table is essential for tracking database changes and should not be modified directly.

## Schema Overview

The database consists of these main tables:

- `Users` - Stores user account information
- `Hosts` - Stores host information with address and contact details
- `Stays` - Records historical stays with hosts
- `DbVersionInfo` - Tracks database schema versions

For a more detailed schema description, see the main documentation at `docs/database.md`.

## Entity Relationships

- One User can have many Hosts (one-to-many)
- One Host can have many Stays (one-to-many)

## Backups

Neon provides automatic backups of your database. You can also use the GitHub Actions "Database Backup" workflow. 