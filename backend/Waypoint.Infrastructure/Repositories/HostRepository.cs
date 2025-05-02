using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Waypoint.Core.Entities;
using Waypoint.Core.Interfaces;
using Waypoint.Infrastructure.Data;

namespace Waypoint.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for Host entity
    /// </summary>
    public class HostRepository : RepositoryBase<Host>, IHostRepository
    {
        public HostRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        
        /// <summary>
        /// Get hosts for a specific user
        /// </summary>
        public async Task<IEnumerable<Host>> GetHostsByUserIdAsync(Guid userId)
        {
            return await _dbContext.Hosts
                .Where(h => h.UserId == userId)
                .OrderBy(h => h.Name)
                .ToListAsync();
        }
        
        /// <summary>
        /// Get hosts within a radius of a location using PostgreSQL's earthdistance
        /// </summary>
        public async Task<IEnumerable<Host>> GetNearbyHostsAsync(decimal latitude, decimal longitude, int radiusMiles)
        {
            // Convert miles to meters (1 mile = 1609.34 meters)
            double radiusMeters = radiusMiles * 1609.34;
            
            // Using EF Core's FromSqlRaw for a raw SQL query that leverages PostgreSQL's earthdistance
            var hosts = await _dbContext.Hosts
                .FromSqlRaw(
                    @"SELECT h.* 
                      FROM ""Hosts"" h 
                      WHERE 
                        h.""Latitude"" IS NOT NULL 
                        AND h.""Longitude"" IS NOT NULL 
                        AND earth_distance(
                            ll_to_earth({0}, {1}), 
                            ll_to_earth(h.""Latitude""::float8, h.""Longitude""::float8)
                        ) <= {2}
                      ORDER BY earth_distance(
                            ll_to_earth({0}, {1}), 
                            ll_to_earth(h.""Latitude""::float8, h.""Longitude""::float8)
                      )",
                    latitude, longitude, radiusMeters)
                .ToListAsync();
            
            return hosts;
        }
        
        /// <summary>
        /// Search hosts by name or location
        /// </summary>
        public async Task<IEnumerable<Host>> SearchHostsAsync(Guid userId, string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            
            return await _dbContext.Hosts
                .Where(h => h.UserId == userId && 
                    (h.Name.ToLower().Contains(searchTerm) ||
                     h.City.ToLower().Contains(searchTerm) ||
                     h.State.ToLower().Contains(searchTerm) ||
                     h.ZipCode.Contains(searchTerm)))
                .OrderBy(h => h.Name)
                .ToListAsync();
        }
        
        /// <summary>
        /// Get host with all associated stays
        /// </summary>
        public async Task<Host?> GetHostWithStaysAsync(Guid hostId)
        {
            return await _dbContext.Hosts
                .Include(h => h.Stays)
                .FirstOrDefaultAsync(h => h.Id == hostId);
        }
    }
} 