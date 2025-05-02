using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Waypoint.Core.Entities;

namespace Waypoint.Core.Interfaces
{
    /// <summary>
    /// Repository interface for Stay entity
    /// </summary>
    public interface IStayRepository : IRepositoryBase<Stay>
    {
        /// <summary>
        /// Get stays for a specific host
        /// </summary>
        Task<IEnumerable<Stay>> GetStaysByHostIdAsync(Guid hostId);
        
        /// <summary>
        /// Get stays for a specific user (across all hosts)
        /// </summary>
        Task<IEnumerable<Stay>> GetStaysByUserIdAsync(Guid userId);
        
        /// <summary>
        /// Get stays within a date range
        /// </summary>
        Task<IEnumerable<Stay>> GetStaysByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? userId = null);
        
        /// <summary>
        /// Get stay with host information
        /// </summary>
        Task<Stay?> GetStayWithHostAsync(Guid stayId);
    }
} 