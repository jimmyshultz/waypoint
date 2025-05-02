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
    /// Repository implementation for Stay entity
    /// </summary>
    public class StayRepository : RepositoryBase<Stay>, IStayRepository
    {
        public StayRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        
        /// <summary>
        /// Get stays for a specific host
        /// </summary>
        public async Task<IEnumerable<Stay>> GetStaysByHostIdAsync(Guid hostId)
        {
            return await _dbContext.Stays
                .Where(s => s.HostId == hostId)
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();
        }
        
        /// <summary>
        /// Get stays for a specific user (across all hosts)
        /// </summary>
        public async Task<IEnumerable<Stay>> GetStaysByUserIdAsync(Guid userId)
        {
            return await _dbContext.Stays
                .Include(s => s.Host)
                .Where(s => s.Host.UserId == userId)
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();
        }
        
        /// <summary>
        /// Get stays within a date range
        /// </summary>
        public async Task<IEnumerable<Stay>> GetStaysByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? userId = null)
        {
            var query = _dbContext.Stays
                .Include(s => s.Host)
                .Where(s => 
                    (s.StartDate >= startDate && s.StartDate <= endDate) ||
                    (s.EndDate >= startDate && s.EndDate <= endDate) ||
                    (s.StartDate <= startDate && s.EndDate >= endDate));
            
            if (userId.HasValue)
            {
                query = query.Where(s => s.Host.UserId == userId.Value);
            }
            
            return await query
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();
        }
        
        /// <summary>
        /// Get stay with host information
        /// </summary>
        public async Task<Stay?> GetStayWithHostAsync(Guid stayId)
        {
            return await _dbContext.Stays
                .Include(s => s.Host)
                .FirstOrDefaultAsync(s => s.Id == stayId);
        }
    }
} 