# Waypoint Database Documentation

## Overview
The Waypoint application uses PostgreSQL for data storage. The database structure is designed to support touring musicians in tracking and managing their network of hosts across the USA.

## Database Schema

### Tables

#### Users
Stores information about the musicians using the application.

| Column        | Data Type      | Constraints           | Description                               |
|---------------|----------------|------------------------|-------------------------------------------|
| Id            | UUID           | PK, NOT NULL          | Unique identifier for each user           |
| Email         | VARCHAR(255)   | UNIQUE, NOT NULL      | User's email address                      |
| PasswordHash  | VARCHAR(255)   | NOT NULL              | Hashed password                           |
| UserName      | VARCHAR(100)   | UNIQUE, NOT NULL      | Username for display                      |
| FirstName     | VARCHAR(100)   | NULL                  | User's first name                         |
| LastName      | VARCHAR(100)   | NULL                  | User's last name                          |
| CreatedAt     | TIMESTAMP      | NOT NULL, DEFAULT NOW | Record creation timestamp                 |
| UpdatedAt     | TIMESTAMP      | NOT NULL, DEFAULT NOW | Record last update timestamp              |

#### Hosts
Stores information about potential places for musicians to stay.

| Column        | Data Type      | Constraints           | Description                               |
|---------------|----------------|------------------------|-------------------------------------------|
| Id            | UUID           | PK, NOT NULL          | Unique identifier for each host           |
| UserId        | UUID           | FK, NOT NULL          | Reference to the user who added this host |
| Name          | VARCHAR(255)   | NOT NULL              | Host's name                               |
| PhoneNumber   | VARCHAR(20)    | NULL                  | Host's contact number                     |
| Email         | VARCHAR(255)   | NULL                  | Host's email address                      |
| AddressLine1  | VARCHAR(255)   | NOT NULL              | First line of address                     |
| AddressLine2  | VARCHAR(255)   | NULL                  | Second line of address (optional)         |
| City          | VARCHAR(100)   | NOT NULL              | City                                      |
| State         | VARCHAR(2)     | NOT NULL              | State (2-letter code)                     |
| ZipCode       | VARCHAR(10)    | NOT NULL              | ZIP code                                  |
| Latitude      | DECIMAL(9,6)   | NULL                  | Geocoded latitude                         |
| Longitude     | DECIMAL(9,6)   | NULL                  | Geocoded longitude                        |
| Notes         | TEXT           | NULL                  | Additional notes about the host           |
| IsActive      | BOOLEAN        | NOT NULL, DEFAULT TRUE| Whether this host is currently active     |
| CreatedAt     | TIMESTAMP      | NOT NULL, DEFAULT NOW | Record creation timestamp                 |
| UpdatedAt     | TIMESTAMP      | NOT NULL, DEFAULT NOW | Record last update timestamp              |

#### Stays
Records historical stays with hosts.

| Column        | Data Type      | Constraints           | Description                               |
|---------------|----------------|------------------------|-------------------------------------------|
| Id            | UUID           | PK, NOT NULL          | Unique identifier for each stay           |
| HostId        | UUID           | FK, NOT NULL          | Reference to the host                     |
| StartDate     | DATE           | NOT NULL              | Start date of the stay                    |
| EndDate       | DATE           | NOT NULL              | End date of the stay                      |
| Rating        | INTEGER        | NULL, CHECK (1-5)     | Rating from 1-5                           |
| Notes         | TEXT           | NULL                  | Notes about the stay                      |
| CreatedAt     | TIMESTAMP      | NOT NULL, DEFAULT NOW | Record creation timestamp                 |
| UpdatedAt     | TIMESTAMP      | NOT NULL, DEFAULT NOW | Record last update timestamp              |

### Indexes

#### Users Table
- Primary Key: `Id`
- Index on `Email` (UNIQUE)
- Index on `UserName` (UNIQUE)

#### Hosts Table
- Primary Key: `Id`
- Foreign Key: `UserId` references `Users(Id)`
- Index on `UserId`
- Spatial index on `(Latitude, Longitude)` for geographic queries
- Combined index on `(City, State)` for location filtering

#### Stays Table
- Primary Key: `Id`
- Foreign Key: `HostId` references `Hosts(Id)`
- Index on `HostId`
- Index on `StartDate` for date-based queries

## Relationships

### Users to Hosts
- One-to-Many: A user can have multiple hosts
- Relationship enforced by foreign key `Hosts.UserId` referencing `Users.Id`
- Cascade delete: When a user is deleted, all their hosts are also deleted

### Hosts to Stays
- One-to-Many: A host can have multiple stays
- Relationship enforced by foreign key `Stays.HostId` referencing `Hosts.Id`
- Cascade delete: When a host is deleted, all associated stays are also deleted

## Database Migration Strategy

### Initial Setup
1. Create base migration for schema creation
2. Include seed data for states and common reference data

### Migration Approach
1. Use Entity Framework Core Code-First migrations to manage schema changes
2. Separate migrations for each major feature
3. Follow naming convention: `YYYYMMDD_FeatureName_ChangeDescription`
4. Include both Up() and Down() methods for all migrations

### Versioning
- Database schema version tracked in a dedicated table
- Migration scripts versioned in source control
- Database changelog maintained in documentation

## Performance Considerations
1. Implement pagination for large result sets
2. Use spatial indexes for geographic queries
3. Consider partitioning Stays table by date range for large datasets
4. Implement proper indexing strategy for common query patterns

## Security Measures
1. No direct access to database from client application
2. All database access through API with proper authentication
3. Sensitive data (like passwords) properly hashed
4. Connection strings stored in secure configuration
5. Regular database backups

## Future Extensions
1. Support for additional tables:
   - `Tours` to plan sequences of visits
   - `TourStops` to link tours with hosts