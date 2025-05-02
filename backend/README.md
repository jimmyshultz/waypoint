# Waypoint Backend

This directory contains the .NET backend solution for the Waypoint application.

## Solution Structure

The solution follows a clean architecture pattern with the following projects:

- **Waypoint.Api**: ASP.NET Core Web API project with controllers and API configuration
- **Waypoint.Core**: Core domain models, interfaces, and business logic
- **Waypoint.Infrastructure**: Data access, repository implementations, and external services
- **Waypoint.Identity**: Authentication and authorization (JWT-based)

## Prerequisites

- .NET 8 SDK or later
- PostgreSQL database (Neon recommended)

## Building the Solution

To build the solution locally:

```bash
cd backend
dotnet restore
dotnet build
```

## Running the API

The easiest way to run the API is using the provided script:

```bash
./scripts/run-api.sh
```

Alternatively, you can run it directly:

```bash
cd backend/Waypoint.Api
dotnet run
```

## Continuous Integration

A GitHub Actions workflow automatically builds the solution on:
- Pushes to the main branch
- Pull requests targeting the main branch

The workflow performs the following steps:
1. Restores dependencies
2. Builds the solution
3. (When tests are added) Runs unit and integration tests

## Database Access

The solution uses Entity Framework Core for database access with:
- Repository pattern for data access abstraction
- Unit of Work pattern for transaction management

## Extending the Backend

### Adding a New Entity

1. Create an entity class in `Waypoint.Core/Entities`
2. Add a repository interface in `Waypoint.Core/Interfaces`
3. Implement the repository in `Waypoint.Infrastructure/Repositories`
4. Add DbSet to `ApplicationDbContext` in `Waypoint.Infrastructure/Data`
5. Register the repository in `DependencyInjection.cs`

### Adding a New API Endpoint

1. Create or modify a controller in `Waypoint.Api/Controllers`
2. Define request/response DTOs in `Waypoint.Core/DTOs` if needed
3. Implement the endpoint logic using the repository interfaces

### Adding Tests (Future)

When adding tests:
1. Create unit tests in a dedicated test project
2. Use xUnit for testing framework
3. Mock external dependencies using Moq or a similar library
4. Uncomment the test section in the CI workflow 