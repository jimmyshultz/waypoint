using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Waypoint.Core.Entities;

namespace Waypoint.Core.Interfaces
{
    /// <summary>
    /// Repository interface for Host entity
    /// </summary>
    public interface IHostRepository : IRepositoryBase<Host>
    {
        /// <summary>
        /// Get hosts for a specific user
        /// </summary>
        Task<IEnumerable<Host>> GetHostsByUserIdAsync(Guid userId);
        
        /// <summary>
        /// Get hosts within a radius of a location
        /// </summary>
        Task<IEnumerable<Host>> GetNearbyHostsAsync(decimal latitude, decimal longitude, int radiusMiles);
        
        /// <summary>
        /// Search hosts by name or location
        /// </summary>
        Task<IEnumerable<Host>> SearchHostsAsync(Guid userId, string searchTerm);
        
        /// <summary>
        /// Get hosts with their stay history
        /// </summary>
        Task<Host?> GetHostWithStaysAsync(Guid hostId);
    }
} 