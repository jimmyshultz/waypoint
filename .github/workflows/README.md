# GitHub Actions Workflows

This directory contains GitHub Actions workflows for the Waypoint application.

## Available Workflows

| Workflow | File | Description |
|----------|------|-------------|
| Apply Migrations | `apply-migrations.yml` | Applies database migrations to the specified environment |
| Revert Migrations | `revert-migrations.yml` | Reverts database migrations (use with caution) |
| Database Query | `database-query.yml` | Runs SQL queries against the database |
| Database Backup | `database-backup.yml` | Creates and stores database backups |
| Development Environment Setup | `dev-environment-setup.yml` | Sets up a development environment |

## Prerequisites

Before using these workflows, you need to set up GitHub Secrets:

1. Go to your repository settings
2. Navigate to "Secrets and variables" > "Actions"
3. Add the following secrets:
   - `DB_CONNECTION_STRING` - PostgreSQL connection string for each environment
   - Other secrets as needed for your specific deployment

## Usage

### Apply Migrations

Triggered automatically when changes are pushed to the `migrations` directory, or can be run manually.

To run manually:
1. Go to the Actions tab
2. Select "Apply Database Migrations"
3. Click "Run workflow"
4. Select the target environment (development/staging/production)
5. Click "Run workflow"

### Revert Migrations

Use with extreme caution as this will delete all data in the database.

To run:
1. Go to the Actions tab
2. Select "Revert Database Migrations"
3. Click "Run workflow"
4. Select the target environment
5. Type "I UNDERSTAND" to confirm
6. Click "Run workflow"

### Database Query

Run SQL queries against the database.

To run:
1. Go to the Actions tab
2. Select "Run Database Query"
3. Click "Run workflow"
4. Select the target environment
5. Enter your SQL query
6. Select whether this is a read-only query
7. Click "Run workflow"

### Database Backup

Automatically runs daily, or can be run manually.

To run manually:
1. Go to the Actions tab
2. Select "Database Backup"
3. Click "Run workflow"
4. Select the target environment
5. Click "Run workflow"

### Development Environment Setup

Sets up a development environment with required documentation.

To run:
1. Go to the Actions tab
2. Select "Setup Development Environment"
3. Click "Run workflow"

## Environments

This project uses GitHub Environments for managing different deployment targets:

- **Development**: For development and testing
- **Staging**: For pre-production testing
- **Production**: For the live application

Each environment has its own secrets and variables, which are used by the workflows. 