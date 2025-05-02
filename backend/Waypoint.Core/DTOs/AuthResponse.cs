using System;

namespace Waypoint.Core.DTOs
{
    /// <summary>
    /// Data transfer object for authentication responses
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// User's email address
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Username (display name)
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        
        /// <summary>
        /// JWT token for authentication
        /// </summary>
        public string Token { get; set; } = string.Empty;
        
        /// <summary>
        /// Refresh token for generating new JWT tokens
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
        
        /// <summary>
        /// When the token expires
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
} 