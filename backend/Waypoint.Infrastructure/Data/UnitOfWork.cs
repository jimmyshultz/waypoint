using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Waypoint.Core.Interfaces;
using Waypoint.Infrastructure.Repositories;

namespace Waypoint.Infrastructure.Data
{
    /// <summary>
    /// Unit of work implementation to manage transactions across repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction? _transaction;
        private bool _disposed;
        
        private IHostRepository? _hostRepository;
        private IStayRepository? _stayRepository;
        
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        /// <summary>
        /// Host repository
        /// </summary>
        public IHostRepository Hosts => _hostRepository ??= new HostRepository(_dbContext);
        
        /// <summary>
        /// Stay repository
        /// </summary>
        public IStayRepository Stays => _stayRepository ??= new StayRepository(_dbContext);
        
        /// <summary>
        /// Save changes to the database
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        
        /// <summary>
        /// Begin a new transaction
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }
        
        /// <summary>
        /// Commit the current transaction
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }
        
        /// <summary>
        /// Rollback the current transaction
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        
        /// <summary>
        /// Dispose the context and transaction
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Dispose the context and transaction
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transaction?.Dispose();
                _dbContext.Dispose();
                _disposed = true;
            }
        }
    }
} 