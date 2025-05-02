# Waypoint Development Guide

This document provides comprehensive guidance for setting up and working with the Waypoint development environment.

## Prerequisites

Before you begin, ensure you have the following installed:

- **Git**: Version control system
- **PostgreSQL**: Database server (local installation or cloud service)
- **Node.js**: JavaScript runtime (for frontend development)
- **.NET Core SDK**: For backend development (coming soon)
- **Docker** (optional): For containerized development

## Initial Setup

### Option 1: Using GitHub Actions (Recommended)

1. Fork/clone the repository:
   ```bash
   git clone https://github.com/yourusername/waypoint.git
   cd waypoint
   ```

2. Navigate to the "Actions" tab in your GitHub repository
3. Run the "Setup Development Environment" workflow
4. Follow the on-screen instructions to complete the setup

### Option 2: Manual Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/waypoint.git
   cd waypoint
   ```

2. Create and set up your `.env` file:
   ```bash
   cp .env.example .env
   # Edit the .env file with your credentials
   ```

3. Set up your database:
   - Create a PostgreSQL database
   - Update the `.env` file with your database connection information
   - Use the GitHub Actions "Apply Database Migrations" workflow to apply migrations

## Working with GitHub Actions Workflows

We've implemented several GitHub Actions workflows to simplify development tasks:

### Database Management

#### Apply Migrations
1. Navigate to the Actions tab in GitHub
2. Select "Apply Database Migrations" workflow
3. Click "Run workflow"
4. Enter the required parameters and run

#### Revert Migrations
1. Navigate to the Actions tab in GitHub
2. Select "Revert Database Migrations" workflow
3. Click "Run workflow"
4. Specify the migration to revert to and run

#### Database Queries
1. Navigate to the Actions tab in GitHub
2. Select "Run SQL Query" workflow
3. Click "Run workflow"
4. Enter your SQL query and run

#### Database Backups
1. Navigate to the Actions tab in GitHub
2. Select "Create Database Backup" workflow
3. Click "Run workflow"

## Local Development (Coming Soon)

### Backend Development

Instructions for setting up and running the .NET Core backend will be provided when the backend implementation is ready.

### Frontend Development

Instructions for setting up and running the React frontend will be provided when the frontend implementation is ready.

## Best Practices

### Environment Variables

- Never commit your `.env` file to version control
- Always use the `.env.example` file as a template
- Update the `.env.example` file when adding new environment variables

### Database Migrations

- Always test migrations locally before applying them to production
- Create both "up" and "down" migrations to ensure reversibility
- Use descriptive names for migration files

### Code Style

- Follow the established code style and conventions
- Run linters before committing code
- Write tests for new features

## Troubleshooting

### Common Issues

#### Database Connection Issues
- Verify that your PostgreSQL server is running
- Check that your database credentials in `.env` are correct
- Ensure that your database exists and is accessible

#### GitHub Actions Issues
- Check that your GitHub Secrets are properly configured
- Verify that you have the necessary permissions to run workflows
- Review the workflow logs for error messages

## Getting Help

If you encounter issues not covered in this guide, please:

1. Check the existing GitHub Issues
2. Create a new issue with detailed information about your problem
3. Contact the project maintainers 