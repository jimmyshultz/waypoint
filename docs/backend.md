# Waypoint Backend Documentation

## Overview

The Waypoint backend is built using ASP.NET Core 8, organized into a multi-project solution that follows clean architecture principles. The backend provides a RESTful API for the frontend client to interact with the database.

## Project Structure

The backend solution consists of the following projects:

- **Waypoint.Api**: The ASP.NET Core Web API project that handles HTTP requests and responses
- **Waypoint.Core**: Contains domain entities, interfaces, DTOs, and business logic
- **Waypoint.Infrastructure**: Implements data access and external service integrations
- **Waypoint.Identity**: Handles authentication and authorization (JWT-based)

## Getting Started

### Prerequisites

- .NET 8 SDK or later
- PostgreSQL database (Neon recommended)

### Running the API

Use the provided script to run the API:

```bash
./scripts/run-api.sh
```

The API will be available at http://localhost:5258.

## Configuration

Configuration is managed through appsettings.json files and environment variables:

- `appsettings.json`: Base configuration
- `appsettings.Development.json`: Development-specific configuration
- Environment variables: Overrides settings from appsettings files

### Key Configuration Settings

- **Database Connection**: Set via the `DB_CONNECTION_STRING` environment variable
- **JWT Authentication**: Configured with:
  - `JWT_SECRET`: Secret key for token generation/validation
  - `JWT_ISSUER`: Token issuer (typically the API domain)
  - `JWT_AUDIENCE`: Token audience (typically the client app)
  - `JWT_EXPIRY_MINUTES`: Token lifetime in minutes

## API Endpoints

### Current Test Endpoints

The following endpoints are available for testing the backend functionality:

- `GET /api/test/db-connection`: Tests database connectivity
- `GET /api/test/db-status`: Shows database status information
- `GET /api/test/create-tables`: Creates database tables
- `GET /api/test/seed-test-data`: Creates sample test data
- `GET /api/test/hosts`: Lists all hosts
- `GET /api/test/stays`: Lists all stays

### Planned Endpoints

The following endpoints are planned for the completed application:

#### Authentication

- `POST /api/auth/register`: Registers a new user
- `POST /api/auth/login`: Authenticates a user and returns a JWT
- `POST /api/auth/refresh-token`: Refreshes an expired JWT
- `POST /api/auth/logout`: Invalidates a JWT

#### Hosts

- `GET /api/hosts`: Lists all hosts for the authenticated user
- `GET /api/hosts/{id}`: Gets a specific host
- `POST /api/hosts`: Creates a new host
- `PUT /api/hosts/{id}`: Updates an existing host
- `DELETE /api/hosts/{id}`: Deletes a host
- `GET /api/hosts/nearby`: Finds hosts near a location

#### Stays

- `GET /api/stays`: Lists all stays for the authenticated user
- `GET /api/stays/{id}`: Gets a specific stay
- `POST /api/stays`: Creates a new stay
- `PUT /api/stays/{id}`: Updates an existing stay
- `DELETE /api/stays/{id}`: Deletes a stay
- `GET /api/hosts/{hostId}/stays`: Lists all stays at a specific host

## Architecture

### Domain Entities

The core domain entities include:

- **ApplicationUser**: Represents a user of the application
- **Host**: Represents a potential place to stay
- **Stay**: Represents a historical stay at a host

### Data Access

- Entity Framework Core for ORM
- Repository pattern for data access abstraction
- Unit of Work pattern for transaction management

### Authentication

- ASP.NET Core Identity for user management
- JWT tokens for API authentication
- Role-based authorization

## Future Plans

The following features are planned for future development:

- Complete authentication implementation
- Full CRUD operations for all entities
- Pagination for list endpoints
- Advanced filtering and search capabilities
- Image upload for hosts
- Geographic search and filtering 