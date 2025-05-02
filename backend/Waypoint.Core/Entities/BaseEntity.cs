namespace Waypoint.Core.Entities
{
    /// <summary>
    /// Base entity class that all other entities inherit from
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Unique identifier for the entity
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// When the entity was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// When the entity was last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
} 