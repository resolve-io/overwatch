using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Overwatch.FeatureFlag.Api.Persistence;

[Table("Feature")] // Maps this class to the "Feature" table
public record FeatureEntity : EntityBase, IEntityTypeConfiguration<FeatureEntity>
{
    /// <summary>
    /// Gets or sets the name of the feature.
    /// </summary>
    [Required] // Ensures this field is not nullable
    [MaxLength(255)] // Sets a maximum length for the column
    public string Name { get; init; }

    /// <summary>
    /// Navigation property for associated rules.
    /// </summary>
    public IList<RuleEntity> Rules { get; init; } = new List<RuleEntity>();

    /// <summary>
    /// Configures the entity in the EF model builder.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<FeatureEntity> builder)
    {
        // Table configuration
        builder.ToTable("Feature");

        // Primary key
        builder.HasKey(f => f.Id);

        // Property configurations
        builder.Property(f => f.Name)
            .IsRequired() // Marks as not nullable
            .HasMaxLength(255); // Limits string length

        // Relationships
        builder.HasMany(f => f.Rules)
            .WithOne(r => r.Feature) // Assumes a Feature property exists on RuleEntity
            .HasForeignKey(r => r.FeatureId) // Assumes FeatureId exists on RuleEntity
            .OnDelete(DeleteBehavior.Cascade); // Cascades on delete
    }
}