# Waypoint Backend Documentation

## Overview
The Waypoint backend is built with C# using ASP.NET Core. It provides a RESTful API for the frontend to interact with the PostgreSQL database, handles authentication, and implements business logic for the application.

## Project Architecture

### Solution Structure
```
Waypoint.sln
├── Waypoint.Api                 # Web API project
├── Waypoint.Core                # Core business logic and domain models
├── Waypoint.Infrastructure      # Data access and external services
├── Waypoint.Identity            # Authentication and authorization
└── Waypoint.Tests               # Unit and integration tests
```

### Layer Responsibilities

#### API Layer (Waypoint.Api)
- Controllers for handling HTTP requests
- Request/response models (DTOs)
- API configurations (CORS, routing, etc.)
- Dependency injection setup
- Middleware configuration

#### Core Layer (Waypoint.Core)
- Domain entities (matching database schema)
- Interfaces for repositories and services
- Business logic services
- Custom exceptions
- Domain validation

#### Infrastructure Layer (Waypoint.Infrastructure)
- Entity Framework Core DbContext
- Repository implementations
- Database migrations
- External service integrations (e.g., geocoding)
- Caching implementation

#### Identity Layer (Waypoint.Identity)
- ASP.NET Core Identity implementation
- JWT token generation and validation
- User management services
- Authentication policies

## API Endpoints

### Authentication

| Endpoint | Method | Request Body | Response | Description |
|----------|--------|--------------|----------|-------------|
| `/api/auth/register` | POST | `{ email, username, password }` | `{ id, email, username, token }` | Register a new user |
| `/api/auth/login` | POST | `{ email, password }` | `{ id, email, username, token }` | Authenticate and get JWT token |
| `/api/auth/refresh-token` | POST | `{ refreshToken }` | `{ token, refreshToken }` | Refresh an expired JWT token |
| `/api/auth/logout` | POST | - | `204 No Content` | Logout and invalidate tokens |

### Hosts

| Endpoint | Method | Request Body/Parameters | Response | Description |
|----------|--------|-------------------------|----------|-------------|
| `/api/hosts` | GET | Query params: `page`, `pageSize`, `searchTerm` | List of host objects with pagination metadata | Get all hosts for current user |
| `/api/hosts/{id}` | GET | - | Host object | Get a specific host by ID |
| `/api/hosts` | POST | Host object | Created host object | Create a new host |
| `/api/hosts/{id}` | PUT | Host object | Updated host object | Update an existing host |
| `/api/hosts/{id}` | DELETE | - | `204 No Content` | Delete a host |
| `/api/hosts/nearby` | GET | Query params: `lat`, `lng`, `radius` | List of nearby hosts | Find hosts within radius (miles) |

### Stays

| Endpoint | Method | Request Body/Parameters | Response | Description |
|----------|--------|-------------------------|----------|-------------|
| `/api/stays` | GET | Query params: `page`, `pageSize` | List of stay objects with pagination metadata | Get all stays for current user |
| `/api/stays/{id}` | GET | - | Stay object | Get a specific stay by ID |
| `/api/stays` | POST | Stay object | Created stay object | Create a new stay |
| `/api/stays/{id}` | PUT | Stay object | Updated stay object | Update an existing stay |
| `/api/stays/{id}` | DELETE | - | `204 No Content` | Delete a stay |
| `/api/hosts/{hostId}/stays` | GET | - | List of stays for host | Get all stays for a specific host |

## Request/Response Models (DTOs)

### Authentication DTOs

```csharp
public class RegisterRequest
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class AuthResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
```

### Host DTOs

```csharp
public class HostDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateHostRequest
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Notes { get; set; }
}

public class UpdateHostRequest
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Notes { get; set; }
    public bool IsActive { get; set; }
}
```

### Stay DTOs

```csharp
public class StayDto
{
    public Guid Id { get; set; }
    public Guid HostId { get; set; }
    public string HostName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? Rating { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateStayRequest
{
    public Guid HostId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? Rating { get; set; }
    public string Notes { get; set; }
}

public class UpdateStayRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? Rating { get; set; }
    public string Notes { get; set; }
}
```

## Entity Models

### User Entity

```csharp
public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public virtual ICollection<Host> Hosts { get; set; }
}
```

### Host Entity

```csharp
public class Host
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<Stay> Stays { get; set; }
}
```

### Stay Entity

```csharp
public class Stay
{
    public Guid Id { get; set; }
    public Guid HostId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? Rating { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public virtual Host Host { get; set; }
}
```

## Authentication Implementation

### JWT Configuration

```csharp
public class JwtSettings
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryMinutes { get; set; }
}
```

### Token Generation

```csharp
public async Task<string> GenerateJwtToken(ApplicationUser user)
{
    var userClaims = await _userManager.GetClaimsAsync(user);
    var roles = await _userManager.GetRolesAsync(user);
    
    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("userName", user.UserName)
    };
    
    // Add roles as claims
    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
    claims.AddRange(userClaims);
    
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
        SigningCredentials = creds,
        Issuer = _jwtSettings.Issuer,
        Audience = _jwtSettings.Audience
    };
    
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    
    return tokenHandler.WriteToken(token);
}
```

## Database Context

```csharp
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Host> Hosts { get; set; }
    public DbSet<Stay> Stays { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure entity relationships
        modelBuilder.Entity<Host>()
            .HasOne(h => h.User)
            .WithMany(u => u.Hosts)
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Stay>()
            .HasOne(s => s.Host)
            .WithMany(h => h.Stays)
            .HasForeignKey(s => s.HostId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Configure property constraints
        modelBuilder.Entity<Stay>()
            .Property(s => s.Rating)
            .HasAnnotation("CheckConstraint", "Rating >= 1 AND Rating <= 5");
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update timestamps
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;
            
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
            
            entity.UpdatedAt = DateTime.UtcNow;
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}
```

## Exception Handling

```csharp
// Global exception handling middleware
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case ValidationException validationEx:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new
                {
                    title = "Validation Failed",
                    status = (int)code,
                    errors = validationEx.Errors
                });
                break;
            case NotFoundException _:
                code = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new
                {
                    title = "Resource Not Found",
                    status = (int)code,
                });
                break;
            case UnauthorizedAccessException _:
                code = HttpStatusCode.Unauthorized;
                result = JsonSerializer.Serialize(new
                {
                    title = "Unauthorized",
                    status = (int)code,
                });
                break;
            default:
                logger.LogError(exception, "Server Error");
                result = JsonSerializer.Serialize(new
                {
                    title = "Server Error",
                    status = (int)code,
                });
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        await context.Response.WriteAsync(result);
    }
}
```

## Security Considerations

1. **Input Validation**:
   - Use Data Annotations and FluentValidation for request validation
   - Validate all user inputs before processing

2. **Authentication and Authorization**:
   - JWT tokens with appropriate expiry
   - Refresh token rotation for security
   - Role-based authorization using ASP.NET Core policies

3. **Data Protection**:
   - Password hashing via ASP.NET Core Identity
   - Sensitive data encryption using .NET Data Protection APIs
   - HTTPS enforcement

4. **Additional Security Measures**:
   - API rate limiting to prevent abuse
   - Security headers (HSTS, XSS Protection, etc.)
   - CORS policy configuration
   - SQL injection protection via parameterized queries

## Testing Strategy

1. **Unit Tests**:
   - Test individual components in isolation
   - Mock external dependencies
   - Focus on business logic and validation

2. **Integration Tests**:
   - Test API endpoints with in-memory database
   - Validate request/response cycles
   - Test authentication flows

3. **End-to-End Tests**:
   - Test complete workflows
   - Use real database with test data

## Logging and Monitoring

1. **Logging**:
   - Use Serilog for structured logging
   - Log all API requests and responses
   - Log exceptions with stack traces
   - Different log levels based on environment

2. **Monitoring**:
   - Application metrics collection
   - Health checks for dependencies
   - Performance monitoring

## Deployment Considerations

1. **Environment Configuration**:
   - Environment-specific appsettings.json files
   - Secrets management using Azure Key Vault
   - Feature flags for controlled rollout

2. **Containerization**:
   - Docker support for consistent deployment
   - Docker Compose for local development
   - Kubernetes for orchestration (optional)

3. **CI/CD Pipeline**:
   - Automated builds with GitHub Actions
   - Automated testing
   - Deployment to Azure App Service 