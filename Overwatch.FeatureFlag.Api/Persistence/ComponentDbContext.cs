using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Overwatch.FeatureFlag.Api.Persistence
{
    [ExcludeFromCodeCoverage]
    public class ComponentDbContext : DbContext
    {
        public ComponentDbContext(DbContextOptions<ComponentDbContext> options) : base(options)
        {
            // Set NoTracking as default for all queries
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<FeatureEntity> Features { get; set; }

        public DbSet<EnvironmentEntity> Environments { get; set; }

        public DbSet<RuleEntity> Rules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Entities Implementing IEntityTypeConfiguration<T>: Automatically configured
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<EntityBase>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DateCreated = DateTime.UtcNow;
                    entry.Entity.DateModified = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.DateModified = DateTime.UtcNow;
                }
            }
        }

    }
}
