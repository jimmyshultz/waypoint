using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Waypoint.Core.Entities;

namespace Waypoint.Core.Interfaces
{
    /// <summary>
    /// Generic repository interface for CRUD operations
    /// </summary>
    /// <typeparam name="T">Entity type that inherits from BaseEntity</typeparam>
    public interface IRepositoryBase<T> where T : BaseEntity
    {
        /// <summary>
        /// Get all entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();
        
        /// <summary>
        /// Get entities by condition
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        
        /// <summary>
        /// Get entity by id
        /// </summary>
        Task<T?> GetByIdAsync(Guid id);
        
        /// <summary>
        /// Add a new entity
        /// </summary>
        Task<T> AddAsync(T entity);
        
        /// <summary>
        /// Update an existing entity
        /// </summary>
        Task UpdateAsync(T entity);
        
        /// <summary>
        /// Delete an entity
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Check if entity exists
        /// </summary>
        Task<bool> ExistsAsync(Guid id);
    }
} 