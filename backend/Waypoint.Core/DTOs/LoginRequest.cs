namespace Waypoint.Core.DTOs
{
    /// <summary>
    /// Data transfer object for login requests
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// User's email address
        /// </summary>
        public required string Email { get; set; }
        
        /// <summary>
        /// User's password
        /// </summary>
        public required string Password { get; set; }
    }
} 