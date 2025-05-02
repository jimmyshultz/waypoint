using System;

namespace Waypoint.Core.Entities
{
    /// <summary>
    /// Represents a stay at a host's location
    /// </summary>
    public class Stay : BaseEntity
    {
        /// <summary>
        /// Reference to the host
        /// </summary>
        public Guid HostId { get; set; }
        
        /// <summary>
        /// Start date of the stay
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the stay
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Rating from 1-5
        /// </summary>
        public int? Rating { get; set; }
        
        /// <summary>
        /// Notes about the stay
        /// </summary>
        public string? Notes { get; set; }
        
        /// <summary>
        /// Reference to the host
        /// </summary>
        public virtual Host? Host { get; set; }
    }
} 