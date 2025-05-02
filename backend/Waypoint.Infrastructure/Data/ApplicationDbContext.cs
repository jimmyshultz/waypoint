using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Waypoint.Core.Entities;

namespace Waypoint.Infrastructure.Data
{
    /// <summary>
    /// Entity Framework Core DbContext for the application
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Host> Hosts { get; set; }
        public DbSet<Stay> Stays { get; set; }
        
        /// <summary>
        /// Configure entity relationships and constraints
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Host entity
            modelBuilder.Entity<Host>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.AddressLine1).IsRequired().HasMaxLength(255);
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);
                entity.Property(e => e.State).IsRequired().HasMaxLength(2);
                entity.Property(e => e.ZipCode).IsRequired().HasMaxLength(10);
                
                // Configure one-to-many relationship with Stay
                entity.HasMany(e => e.Stays)
                      .WithOne(e => e.Host)
                      .HasForeignKey(e => e.HostId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configure Stay entity
            modelBuilder.Entity<Stay>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
                
                // Add a check constraint for Rating (1-5)
                entity.HasCheckConstraint("CK_Stay_Rating", "\"Rating\" BETWEEN 1 AND 5 OR \"Rating\" IS NULL");
            });
            
            // Add version tracking table
            modelBuilder.Entity<DbVersionInfo>(entity =>
            {
                entity.HasKey(e => e.Version);
                entity.Property(e => e.Version).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.AppliedAt).IsRequired();
            });
        }
        
        /// <summary>
        /// Override SaveChanges to automatically set CreatedAt and UpdatedAt
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
            
            return base.SaveChangesAsync(cancellationToken);
        }
    }
    
    /// <summary>
    /// Tracks database schema versions
    /// </summary>
    public class DbVersionInfo
    {
        public string Version { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime AppliedAt { get; set; }
    }
} 