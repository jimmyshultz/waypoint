using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Waypoint.Core.Interfaces;
using Waypoint.Infrastructure.Data;
using Waypoint.Infrastructure.Repositories;

namespace Waypoint.Infrastructure
{
    /// <summary>
    /// Extension methods for setting up infrastructure services in an IServiceCollection
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Add infrastructure services to the service collection
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add database context
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection") 
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                
                Console.WriteLine($"Using connection string: {connectionString}");
                
                try
                {
                    options.UseNpgsql(connectionString, npgsqlOptions =>
                    {
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null);
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error configuring database: {ex.Message}");
                    throw;
                }
            });
            
            // Add repositories
            services.AddScoped<IHostRepository, HostRepository>();
            services.AddScoped<IStayRepository, StayRepository>();
            
            // Add unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return services;
        }
    }
} 