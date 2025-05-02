using System;
using System.Threading.Tasks;

namespace Waypoint.Core.Interfaces
{
    /// <summary>
    /// Unit of work interface to manage transactions across repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Host repository
        /// </summary>
        IHostRepository Hosts { get; }
        
        /// <summary>
        /// Stay repository
        /// </summary>
        IStayRepository Stays { get; }
        
        /// <summary>
        /// Save changes to the database
        /// </summary>
        Task<int> SaveChangesAsync();
        
        /// <summary>
        /// Begin a new transaction
        /// </summary>
        Task BeginTransactionAsync();
        
        /// <summary>
        /// Commit the current transaction
        /// </summary>
        Task CommitTransactionAsync();
        
        /// <summary>
        /// Rollback the current transaction
        /// </summary>
        Task RollbackTransactionAsync();
    }
} 