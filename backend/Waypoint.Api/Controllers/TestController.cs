using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Waypoint.Core.Entities;
using Waypoint.Core.Interfaces;
using Waypoint.Infrastructure.Data;

namespace Waypoint.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TestController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public TestController(IUnitOfWork unitOfWork, ILogger<TestController> logger, ApplicationDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("db-connection")]
        public async Task<IActionResult> TestDatabaseConnection()
        {
            try
            {
                // Try to begin a transaction to verify db connection
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.RollbackTransactionAsync();
                
                return Ok(new { message = "Database connection successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database connection test failed");
                return StatusCode(500, new { error = "Database connection failed", details = ex.Message });
            }
        }

        [HttpGet("db-status")]
        public async Task<IActionResult> GetDatabaseStatus()
        {
            try
            {
                var tableCount = 0;
                var tablesList = new List<string>();

                try
                {
                    // Use safer SQL approach
                    using var command = _dbContext.Database.GetDbConnection().CreateCommand();
                    command.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'";
                    
                    await _dbContext.Database.OpenConnectionAsync();
                    
                    using var result = await command.ExecuteReaderAsync();
                    while (await result.ReadAsync())
                    {
                        tableCount++;
                        tablesList.Add(result.GetString(0));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to get table details");
                }

                return Ok(new 
                { 
                    message = "Database status", 
                    canConnect = true,
                    tableCount = tableCount,
                    tables = tablesList,
                    database = _dbContext.Database.GetDbConnection().Database,
                    server = _dbContext.Database.GetDbConnection().DataSource
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database status check failed");
                return StatusCode(500, new { error = "Database status check failed", details = ex.ToString() });
            }
        }

        [HttpGet("create-tables")]
        public async Task<IActionResult> CreateTables()
        {
            try
            {
                // Directly create tables with SQL rather than relying on EF migrations
                // Note: This is for quick testing only, not recommended for production
                var tablesCreated = new List<string>();
                
                try
                {
                    // Create Users table
                    await _dbContext.Database.ExecuteSqlRawAsync(@"
                        CREATE TABLE IF NOT EXISTS ""Users"" (
                            ""Id"" UUID PRIMARY KEY,
                            ""Email"" VARCHAR(255) NOT NULL,
                            ""PasswordHash"" VARCHAR(255) NOT NULL,
                            ""UserName"" VARCHAR(100) NOT NULL,
                            ""FirstName"" VARCHAR(100) NULL,
                            ""LastName"" VARCHAR(100) NULL,
                            ""CreatedAt"" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                            ""UpdatedAt"" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                        );
                    ");
                    tablesCreated.Add("Users");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create Users table");
                    return StatusCode(500, new { error = "Failed to create Users table", details = ex.ToString() });
                }
                
                try
                {
                    // Create Hosts table
                    await _dbContext.Database.ExecuteSqlRawAsync(@"
                        CREATE TABLE IF NOT EXISTS ""Hosts"" (
                            ""Id"" UUID PRIMARY KEY,
                            ""UserId"" UUID NOT NULL,
                            ""Name"" VARCHAR(255) NOT NULL,
                            ""PhoneNumber"" VARCHAR(20) NULL,
                            ""Email"" VARCHAR(255) NULL,
                            ""AddressLine1"" VARCHAR(255) NOT NULL,
                            ""AddressLine2"" VARCHAR(255) NULL,
                            ""City"" VARCHAR(100) NOT NULL,
                            ""State"" VARCHAR(2) NOT NULL,
                            ""ZipCode"" VARCHAR(10) NOT NULL,
                            ""Latitude"" DECIMAL(9,6) NULL,
                            ""Longitude"" DECIMAL(9,6) NULL,
                            ""Notes"" TEXT NULL,
                            ""IsActive"" BOOLEAN NOT NULL DEFAULT TRUE,
                            ""CreatedAt"" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                            ""UpdatedAt"" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                            FOREIGN KEY (""UserId"") REFERENCES ""Users"" (""Id"")
                        );
                    ");
                    tablesCreated.Add("Hosts");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create Hosts table");
                    return StatusCode(500, new { error = "Failed to create Hosts table", details = ex.ToString() });
                }
                
                try
                {
                    // Create Stays table
                    await _dbContext.Database.ExecuteSqlRawAsync(@"
                        CREATE TABLE IF NOT EXISTS ""Stays"" (
                            ""Id"" UUID PRIMARY KEY,
                            ""HostId"" UUID NOT NULL,
                            ""StartDate"" DATE NOT NULL,
                            ""EndDate"" DATE NOT NULL,
                            ""Rating"" INTEGER NULL CHECK (""Rating"" BETWEEN 1 AND 5),
                            ""Notes"" TEXT NULL,
                            ""CreatedAt"" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                            ""UpdatedAt"" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                            FOREIGN KEY (""HostId"") REFERENCES ""Hosts"" (""Id"")
                        );
                    ");
                    tablesCreated.Add("Stays");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create Stays table");
                    return StatusCode(500, new { error = "Failed to create Stays table", details = ex.ToString() });
                }
                
                try
                {
                    // First drop the version table if it exists
                    await _dbContext.Database.ExecuteSqlRawAsync(@"
                        DROP TABLE IF EXISTS ""DbVersionInfo"";
                    ");
                    
                    // Create DbVersionInfo table
                    await _dbContext.Database.ExecuteSqlRawAsync(@"
                        CREATE TABLE ""DbVersionInfo"" (
                            ""Version"" VARCHAR(20) PRIMARY KEY,
                            ""Description"" VARCHAR(255) NULL,
                            ""AppliedAt"" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                        );
                    ");
                    
                    // Insert the version record without referencing AppliedAt which will use the default value
                    await _dbContext.Database.ExecuteSqlRawAsync(@"
                        INSERT INTO ""DbVersionInfo"" (""Version"", ""Description"")
                        VALUES ('1.0.0', 'Initial test schema');
                    ");
                    
                    tablesCreated.Add("DbVersionInfo");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create or update version information");
                    return StatusCode(500, new { error = "Failed to create version table", details = ex.ToString() });
                }
                
                return Ok(new { message = "Tables created successfully", tables = tablesCreated });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Creating tables failed");
                return StatusCode(500, new { error = "Creating tables failed", details = ex.ToString() });
            }
        }

        [HttpGet("seed-test-data")]
        public async Task<IActionResult> SeedTestData()
        {
            try
            {
                // Create a test user first
                var userId = Guid.NewGuid();
                
                try
                {
                    await _dbContext.Database.ExecuteSqlRawAsync(@"
                        INSERT INTO ""Users"" (""Id"", ""Email"", ""PasswordHash"", ""UserName"", ""CreatedAt"", ""UpdatedAt"")
                        VALUES ({0}, 'test@example.com', 'NotARealHash', 'TestUser', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
                        ON CONFLICT (""Id"") DO NOTHING;
                    ", userId);
                    
                    _logger.LogInformation("Test user created successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create test user");
                    return StatusCode(500, new { error = "Failed to create test user", details = ex.ToString() });
                }

                // Create a test host
                var hostId = Guid.NewGuid();
                
                try
                {
                    await _dbContext.Database.ExecuteSqlRawAsync(@"
                        INSERT INTO ""Hosts"" (
                            ""Id"", ""UserId"", ""Name"", ""AddressLine1"", ""City"", ""State"", ""ZipCode"", 
                            ""Latitude"", ""Longitude"", ""IsActive"", ""CreatedAt"", ""UpdatedAt""
                        )
                        VALUES (
                            {0}, {1}, 'Test Host', '123 Main St', 'Nashville', 'TN', '37203',
                            36.1627, -86.7816, TRUE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
                        )
                        ON CONFLICT (""Id"") DO NOTHING;
                    ", hostId, userId);
                    
                    _logger.LogInformation("Test host created successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create test host");
                    return StatusCode(500, new { error = "Failed to create test host", details = ex.ToString() });
                }

                // Create a test stay
                var stayId = Guid.NewGuid();
                var startDate = DateTime.UtcNow.AddDays(-7);
                var endDate = DateTime.UtcNow.AddDays(-5);
                
                try
                {
                    await _dbContext.Database.ExecuteSqlRawAsync(@"
                        INSERT INTO ""Stays"" (
                            ""Id"", ""HostId"", ""StartDate"", ""EndDate"", ""Rating"", ""Notes"", ""CreatedAt"", ""UpdatedAt""
                        )
                        VALUES (
                            {0}, {1}, {2}, {3}, 4, 'Test stay', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
                        )
                        ON CONFLICT (""Id"") DO NOTHING;
                    ", stayId, hostId, startDate, endDate);
                    
                    _logger.LogInformation("Test stay created successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create test stay");
                    return StatusCode(500, new { error = "Failed to create test stay", details = ex.ToString() });
                }

                return Ok(new 
                { 
                    message = "Test data created successfully",
                    user = new { id = userId, email = "test@example.com" },
                    host = new { id = hostId, name = "Test Host" },
                    stay = new { id = stayId, dates = $"{startDate:d} to {endDate:d}" }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Seeding test data failed");
                return StatusCode(500, new { error = "Seeding test data failed", details = ex.ToString() });
            }
        }

        [HttpGet("hosts")]
        public async Task<IActionResult> GetAllHosts()
        {
            try
            {
                var hosts = await _unitOfWork.Hosts.GetAllAsync();
                return Ok(hosts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Getting hosts failed");
                return StatusCode(500, new { error = "Getting hosts failed", details = ex.ToString() });
            }
        }

        [HttpGet("stays")]
        public async Task<IActionResult> GetAllStays()
        {
            try
            {
                var stays = await _unitOfWork.Stays.GetAllAsync();
                return Ok(stays);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Getting stays failed");
                return StatusCode(500, new { error = "Getting stays failed", details = ex.ToString() });
            }
        }
    }
} 