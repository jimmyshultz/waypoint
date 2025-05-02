# Waypoint Application Architecture

## Overview
A web application that helps touring musicians track and manage their network of hosts across the USA. The application allows users to store information about potential hosts, previous stays, and use this data to plan future tours.

## Tech Stack
- **Database**: PostgreSQL
- **Backend**: C# .NET Core
- **Frontend**: React
- **Authentication**: ASP.NET Core Identity

## System Architecture

### 1. Database Layer
- PostgreSQL database with the following main tables:
  - Users (musicians)
  - Hosts (potential places to stay)
  - Stays (historical records of stays)
  - Tours/Events (optional future expansion)

### 2. Backend API Layer
- ASP.NET Core Web API
- Entity Framework Core for ORM
- RESTful endpoints for:
  - User authentication and management
  - Host CRUD operations
  - Stay history tracking
  - Search and filtering functionality
  - (Optional) Tour planning integration

### 3. Frontend Layer
- React Single Page Application (SPA)
- Component-based UI with responsive design
- Mapping functionality for geographic visualization
- State management for application data

### 4. Authentication Layer
- ASP.NET Core Identity for user authentication
- JWT tokens for API authentication
- Role-based access control
- Secure cookie handling

## Database Schema

### Users Table
```
Users
- Id (PK)
- Email
- PasswordHash
- UserName
- CreatedAt
- UpdatedAt
```

### Hosts Table
```
Hosts
- Id (PK)
- UserId (FK to Users)
- Name
- PhoneNumber
- Email
- AddressLine1
- AddressLine2
- City
- State
- ZipCode
- Latitude
- Longitude
- Notes
- CreatedAt
- UpdatedAt
```

### Stays Table
```
Stays
- Id (PK)
- HostId (FK to Hosts)
- StartDate
- EndDate
- Rating (1-5)
- Notes
- CreatedAt
- UpdatedAt
```

## API Endpoints

### Authentication
- POST /api/auth/register
- POST /api/auth/login
- POST /api/auth/refresh-token
- POST /api/auth/logout

### Hosts
- GET /api/hosts
- GET /api/hosts/{id}
- POST /api/hosts
- PUT /api/hosts/{id}
- DELETE /api/hosts/{id}
- GET /api/hosts/nearby?lat={latitude}&lng={longitude}&radius={miles}

### Stays
- GET /api/stays
- GET /api/stays/{id}
- POST /api/stays
- PUT /api/stays/{id}
- DELETE /api/stays/{id}
- GET /api/hosts/{hostId}/stays

## Frontend Components

### Pages
- Login/Register
- Dashboard
- Host List
- Host Detail
- Add/Edit Host
- Map View
- Stay History
- User Settings

### Core Components
- Navigation/Menu
- Host Card
- Map Component
- Search/Filter Bar
- Stay History Timeline
- Rating Input
- Address Form with Geocoding

## Authentication Flow
1. User registers or logs in through the frontend
2. Backend validates credentials and issues JWT token
3. Frontend stores token in secure storage
4. All subsequent API requests include the JWT in Authorization header
5. Backend middleware validates token for protected routes
6. Token refresh mechanism to maintain session

## Data Flow
1. User actions in React trigger API calls
2. API controller methods process requests
3. Entity Framework Core handles database operations
4. Data is returned to frontend as JSON
5. React components render updated UI based on data

## Security Considerations
- HTTPS for all communication
- Password hashing using ASP.NET Core Identity
- JWT token expiration and refresh strategy
- Input validation on both client and server
- SQL injection protection via parameterized queries
- Cross-Origin Resource Sharing (CORS) configuration
- Protection against Cross-Site Request Forgery (CSRF)

## Deployment Options
- Azure App Service or GCP alternative for hosting the .NET backend
- Neon for PostgreSQL
- Vercel for React frontend
- CI/CD pipeline using GitHub Actions

## Future Extension Possibilities
- Integration with calendar systems
- Tour planning optimization
- Host communication tools
- Mobile application version
- Multi-user support for bands/groups

## Development Status

The Waypoint application is under active development. Current progress:

### Documentation
- ✅ Database schema and structure
- ✅ Backend architecture and API design
- ✅ Frontend component design and UI/UX guidelines

### Implementation
- ✅ Database schema created and implemented in Neon PostgreSQL
- 🔄 Backend implementation in progress
- 🔄 Frontend implementation in progress

## Project Structure

```
waypoint/
├── .github/                  # GitHub-related files
│   ├── workflows/            # GitHub Actions workflows
│   │   ├── apply-migrations.yml    # Apply database migrations
│   │   ├── revert-migrations.yml   # Revert database migrations
│   │   ├── database-query.yml      # Run database queries
│   │   ├── database-backup.yml     # Back up the database
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
├── scripts/                  # Minimal local development scripts
│   ├── setup-local-dev.sh    # Local development setup
│   └── README.md             # Scripts documentation
├── .env.example              # Environment variables template
├── .gitignore                # Git ignore rules
├── backend/                  # .NET Core backend (coming soon)
└── frontend/                 # React frontend (coming soon)
```

## Getting Started

### Initial Setup

There are two ways to set up the project:

#### Option 1: Using GitHub Actions (Recommended)

1. Fork/clone the repository
2. Run the "Setup Development Environment" workflow from the Actions tab
3. Set up GitHub Secrets for your database connection strings
4. Run the "Apply Database Migrations" workflow to set up your database

#### Option 2: Local Setup

1. Clone the repository
2. Run `./scripts/setup-local-dev.sh` to set up your local environment
3. Edit the `.env` file with your credentials

### Database Management

Database operations are managed through GitHub Actions workflows:

- **Apply Migrations**: Sets up or updates the database schema
- **Database Query**: Runs SQL queries against your database
- **Database Backup**: Creates and stores backups of your database

For more information, see `.github/workflows/README.md`.