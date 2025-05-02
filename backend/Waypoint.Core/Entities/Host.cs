using System;
using System.Collections.Generic;

namespace Waypoint.Core.Entities
{
    /// <summary>
    /// Represents a host where a musician can stay
    /// </summary>
    public class Host : BaseEntity
    {
        /// <summary>
        /// Reference to the user who added this host
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Host's name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Host's contact number
        /// </summary>
        public string? PhoneNumber { get; set; }
        
        /// <summary>
        /// Host's email address
        /// </summary>
        public string? Email { get; set; }
        
        /// <summary>
        /// First line of address
        /// </summary>
        public string AddressLine1 { get; set; } = string.Empty;
        
        /// <summary>
        /// Second line of address (optional)
        /// </summary>
        public string? AddressLine2 { get; set; }
        
        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; } = string.Empty;
        
        /// <summary>
        /// State (2-letter code)
        /// </summary>
        public string State { get; set; } = string.Empty;
        
        /// <summary>
        /// ZIP code
        /// </summary>
        public string ZipCode { get; set; } = string.Empty;
        
        /// <summary>
        /// Geocoded latitude
        /// </summary>
        public decimal? Latitude { get; set; }
        
        /// <summary>
        /// Geocoded longitude
        /// </summary>
        public decimal? Longitude { get; set; }
        
        /// <summary>
        /// Additional notes about the host
        /// </summary>
        public string? Notes { get; set; }
        
        /// <summary>
        /// Whether this host is currently active
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Reference to the user who added this host
        /// </summary>
        public virtual ApplicationUser? User { get; set; }
        
        /// <summary>
        /// Collection of stays associated with this host
        /// </summary>
        public virtual ICollection<Stay>? Stays { get; set; }
    }
} 