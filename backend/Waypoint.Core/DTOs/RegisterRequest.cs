namespace Waypoint.Core.DTOs
{
    /// <summary>
    /// Data transfer object for user registration requests
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// User's email address
        /// </summary>
        public required string Email { get; set; }
        
        /// <summary>
        /// Username (display name)
        /// </summary>
        public required string UserName { get; set; }
        
        /// <summary>
        /// User's password
        /// </summary>
        public required string Password { get; set; }
        
        /// <summary>
        /// User's first name
        /// </summary>
        public string? FirstName { get; set; }
        
        /// <summary>
        /// User's last name
        /// </summary>
        public string? LastName { get; set; }
    }
} 