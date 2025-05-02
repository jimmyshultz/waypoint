# Local Development Scripts

This directory contains minimal local development scripts. Most functionality has been moved to GitHub Actions workflows.

## Available Scripts

| Script | Description |
|--------|-------------|
| `setup-local-dev.sh` | Sets up a local development environment |

## Usage

```bash
# Set up local development environment
./scripts/setup-local-dev.sh
```

## GitHub Actions Workflows

Most operations are now handled by GitHub Actions workflows:

- **Apply Migrations**: Automatically applies database migrations
- **Revert Migrations**: Reverts database migrations (use with caution)
- **Database Query**: Runs SQL queries against the database
- **Database Backup**: Creates and stores database backups
- **Setup Dev Environment**: Sets up a development environment

To use these workflows:
1. Go to the Actions tab in the GitHub repository
2. Select the workflow you want to run
3. Click "Run workflow"
4. Provide any required inputs

For more information, see `.github/workflows/README.md` and `.github/DEVELOPMENT.md` 