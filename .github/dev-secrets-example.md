# GitHub Secrets Required for Waypoint Development

This document outlines the GitHub Secrets required for running the Waypoint GitHub Actions workflows successfully.

## Setting Up GitHub Secrets

To set up GitHub Secrets:
1. Go to your repository on GitHub
2. Click on "Settings" tab
3. In the left sidebar, click on "Secrets and variables" > "Actions"
4. Click "New repository secret" to add each of the secrets below

## Required Secrets

| Secret Name | Description | Example Value | Required For |
|-------------|-------------|---------------|-------------|
| `DB_CONNECTION_STRING` | PostgreSQL connection string | `postgresql://username:password@hostname:port/database` | All database operations |
| `DB_HOST` | Database hostname | `db.example.com` | Database operations |
| `DB_PORT` | Database port | `5432` | Database operations |
| `DB_NAME` | Database name | `waypoint` | Database operations |
| `DB_USER` | Database username | `postgres` | Database operations |
| `DB_PASSWORD` | Database password | `your_secure_password` | Database operations |
| `JWT_SECRET` | Secret key for JWT token generation | `a_random_secure_string` | Authentication operations |

## Optional Secrets

| Secret Name | Description | Example Value | Required For |
|-------------|-------------|---------------|-------------|
| `MAPBOX_API_KEY` | API key for Mapbox services | `pk.eyJ1IjoiZXhhbXBsZSIsImEiOiJjazV4eTB3aW0wYTBxM2VudGZtamJyY3Q0In0.example` | Geocoding features |
| `SMTP_PASSWORD` | SMTP password for email notifications | `your_smtp_password` | Email notifications |

## Testing Your Setup

Once you've set up these secrets, you can verify they work correctly by running the "Setup Development Environment" workflow from the Actions tab in your repository.

## Security Notes

- Never include actual secret values in your code or in GitHub issues/comments
- Rotate your secrets regularly for improved security
- Use separate development and production secrets

For more information on how to use these secrets with the GitHub Actions workflows, refer to the [Workflows README](.github/workflows/README.md). 