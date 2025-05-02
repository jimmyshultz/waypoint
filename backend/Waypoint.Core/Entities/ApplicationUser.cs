using System;
using System.Collections.Generic;

namespace Waypoint.Core.Entities
{
    /// <summary>
    /// Represents a user in the application (musician)
    /// </summary>
    public class ApplicationUser
    {
        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// User's email address (used for login)
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Hashed password
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;
        
        /// <summary>
        /// Display name for the user
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        
        /// <summary>
        /// User's first name
        /// </summary>
        public string? FirstName { get; set; }
        
        /// <summary>
        /// User's last name
        /// </summary>
        public string? LastName { get; set; }
        
        /// <summary>
        /// When the user was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// When the user was last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        
        /// <summary>
        /// Collection of hosts associated with this user
        /// </summary>
        public virtual ICollection<Host>? Hosts { get; set; }
    }
} 