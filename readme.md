# Waypoint Application

## Overview
A web application that helps touring musicians track and manage their network of hosts across the USA. The application allows users to store information about potential hosts, previous stays, and use this data to plan future tours.

## Tech Stack
- **Database**: PostgreSQL (hosted on Neon)
- **Backend**: C# .NET Core
- **Frontend**: React (coming soon)
- **Authentication**: ASP.NET Core Identity with JWT

## Current Status
The Waypoint application is under active development:

- ✅ Database schema created and implemented
- ✅ .NET Core API project structure set up
- ✅ GitHub Actions workflows for database operations and backend builds
- ✅ Basic test endpoints for database connectivity
- 🔄 Authentication implementation in progress
- 🔄 Frontend implementation not yet started

## Project Structure

```
waypoint/
├── .github/                  # GitHub-related files
│   ├── workflows/            # GitHub Actions workflows
│   │   ├── apply-migrations.yml    # Apply database migrations
│   │   ├── revert-migrations.yml   # Revert database migrations
│   │   ├── database-query.yml      # Run database queries
│   │   ├── database-backup.yml     # Back up the database
│   │   ├── backend-build.yml       # Build .NET backend
│   │   └── dev-environment-setup.yml # Set up development environment
│   ├── DEVELOPMENT.md        # Development setup guide
│   └── workflows/README.md   # Workflows documentation
├── docs/                     # Project documentation
│   ├── database.md           # Database documentation
│   ├── backend.md            # Backend documentation
│   └── frontend.md           # Frontend documentation
├── migrations/               # Database migration scripts
│   ├── 01_initial_schema.sql        # Creates database schema
│   ├── 01_initial_schema_down.sql   # Reverts database schema
│   └── README.md                    # Database documentation
├── scripts/                  # Utility scripts
│   ├── setup-local-dev.sh    # Local development setup script
│   ├── run-api.sh            # Script to run the API locally
│   └── README.md             # Scripts documentation
├── backend/                  # .NET Core backend
│   ├── Waypoint.Api          # API project
│   ├── Waypoint.Core         # Core domain models and interfaces
│   ├── Waypoint.Infrastructure # Data access and external services
│   └── Waypoint.Identity     # Authentication and user management
├── .env.example              # Environment variables template
└── .gitignore                # Git ignore rules
```

## Getting Started

### Prerequisites
- .NET Core SDK 8.0 or later
- PostgreSQL database (or Neon PostgreSQL account)
- Git

### Initial Setup

There are two ways to set up the project:

#### Option 1: Using GitHub Actions (Recommended)

1. Fork/clone the repository
2. Run the "Setup Development Environment" workflow from the Actions tab
3. Set up GitHub Secrets for your database connection strings
4. Run the "Apply Database Migrations" workflow to set up your database

#### Option 2: Local Setup

1. Clone the repository
   ```bash
   git clone https://github.com/yourusername/waypoint.git
   cd waypoint
   ```

2. Run the setup script
   ```bash
   ./scripts/setup-local-dev.sh
   ```

3. Edit the generated `.env` file with your database credentials

### Running the API Locally

Use the provided script to run the API:

```bash
./scripts/run-api.sh
```

The API will be available at http://localhost:5258 with the following test endpoints:

- GET: http://localhost:5258/api/test/db-connection - Tests database connectivity
- GET: http://localhost:5258/api/test/create-tables - Creates database tables
- GET: http://localhost:5258/api/test/seed-test-data - Creates test data
- GET: http://localhost:5258/api/test/hosts - Lists all hosts
- GET: http://localhost:5258/api/test/stays - Lists all stays

## Database Management

Database operations are managed through GitHub Actions workflows:

- **Apply Migrations**: Sets up or updates the database schema
- **Revert Migrations**: Reverts database schema changes
- **Database Query**: Runs SQL queries against your database
- **Database Backup**: Creates and stores backups of your database

## Continuous Integration

The project uses GitHub Actions for continuous integration:

- **Backend Build**: Automatically builds the .NET solution on pushes and pull requests
  - Ensures code compiles successfully
  - Uploads build artifacts
  - (Future) Runs automated tests

For more information, see `.github/workflows/README.md`.

## Contributing

1. Create a new branch for your feature
2. Make your changes
3. Submit a pull request

## License

[MIT License](LICENSE)