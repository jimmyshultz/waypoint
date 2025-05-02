# GitHub Actions Workflows

This directory contains GitHub Actions workflows for automating database and development operations.

## Available Workflows

| Workflow | File | Description |
|----------|------|-------------|
| Apply Migrations | `apply-migrations.yml` | Applies database migrations to setup or update schema |
| Revert Migrations | `revert-migrations.yml` | Reverts the most recent database migration |
| Database Query | `database-query.yml` | Runs SQL queries against the database |
| Database Backup | `database-backup.yml` | Creates and downloads a backup of the database |
| Backend Build | `backend-build.yml` | Builds the .NET backend solution |
| Development Environment Setup | `dev-environment-setup.yml` | Sets up a development environment |

## Using the Workflows

### Apply Migrations

Applies database migration scripts from the `migrations` directory.

1. Go to the Actions tab in the GitHub repository
2. Select "Apply Migrations"
3. Click "Run workflow"
4. Choose the environment (default is development)
5. The workflow will run all migrations in sequence

### Revert Migrations

Reverts the most recent database migration using the corresponding "down" script.

1. Go to the Actions tab in the GitHub repository
2. Select "Revert Migrations"
3. Click "Run workflow"
4. Choose the environment (default is development)
5. The workflow will run the most recent "down" migration script

### Database Query

Runs SQL queries against the database.

1. Go to the Actions tab in the GitHub repository
2. Select "Database Query"
3. Click "Run workflow"
4. Enter your SQL query
5. Choose the environment (default is development)
6. The workflow will execute the query and display the results

### Database Backup

Creates a backup of the database and provides a download link.

1. Go to the Actions tab in the GitHub repository
2. Select "Database Backup"
3. Click "Run workflow"
4. Choose the environment (default is development)
5. The workflow will create a backup and provide a link to download it

### Backend Build

Builds the .NET Core backend solution to verify that the code compiles.

This workflow runs automatically on:
- Pushes to the `main` branch that modify backend code
- Pull requests that modify backend code

You can also run it manually:
1. Go to the Actions tab in the GitHub repository
2. Select "Backend Build"
3. Click "Run workflow"
4. The workflow will build the solution and upload artifacts

When the workflow completes, you can download the build artifacts from the workflow run page.

### Development Environment Setup

Sets up a development environment with required configuration files.

1. Go to the Actions tab in the GitHub repository
2. Select "Development Environment Setup"
3. Click "Run workflow"
4. The workflow will create necessary configuration files

## Required Secrets

These workflows require the following GitHub secrets to be set:

- `DEV_DB_CONNECTION_STRING` - Connection string for the development database
- `PROD_DB_CONNECTION_STRING` - Connection string for the production database (if applicable)
- `JWT_SECRET` - Secret key for JWT token generation and validation
- `JWT_ISSUER` - Issuer for JWT tokens
- `JWT_AUDIENCE` - Audience for JWT tokens
- `JWT_EXPIRY_MINUTES` - JWT token expiry time in minutes

## Environment Selection

Most workflows support selecting the environment at runtime:

- `development` - Uses development database and configuration
- `production` - Uses production database and configuration (if set up)

For more information on setting up your development environment, see `.github/DEVELOPMENT.md`. 