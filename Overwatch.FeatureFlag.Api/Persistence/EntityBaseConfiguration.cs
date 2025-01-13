using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Overwatch.FeatureFlag.Api.Persistence;

public class EntityBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : EntityBase
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        // Configure primary key
        builder.HasKey(e => e.Id);

        // Configure properties
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd(); // Identity by default

        builder.Property(e => e.DateCreated)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()") // Default value at the time of creation
            .ValueGeneratedOnAdd(); // Set only on add

        builder.Property(e => e.DateModified)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()") // Default value at the time of modification
            .ValueGeneratedOnAddOrUpdate(); // Set on add or update
    }
}