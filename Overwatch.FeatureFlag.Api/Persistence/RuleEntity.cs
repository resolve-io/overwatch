using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Overwatch.FeatureFlag.Api.Persistence;

[Table("Rule")] // Maps this class to the "Rule" table
public record RuleEntity : EntityBase, IEntityTypeConfiguration<RuleEntity>
{
    /// <summary>
    /// The ID of the environment this rule applies to, or NULL for all.
    /// </summary>
    public Guid? EnvironmentId { get; init; }

    /// <summary>
    /// The ID (not the name) of the feature this rule applies to.
    /// </summary>
    [Required] // Ensures the property is not nullable
    public Guid FeatureId { get; init; }

    /// <summary>
    /// The name of the tenant (e.g., "platform" or "bparks") this rule applies to, or NULL for all.
    /// </summary>
    [MaxLength(255)] // Sets a maximum length for the column
    public string Tenant { get; init; }

    /// <summary>
    /// Indicates whether the rule is enabled.
    /// </summary>
    [Required] // Ensures the property is not nullable
    public bool IsEnabled { get; init; }

    /// <summary>
    /// A rule belongs to an environment.
    /// </summary>
    public EnvironmentEntity? Environment { get; init; }

    /// <summary>
    /// A rule belongs to a features
    /// </summary>
    [Required] // Ensures the feature relationship is not nullable
    public FeatureEntity? Feature { get; init; }

    /// <summary>
    /// Configures the entity using the EF model builder.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<RuleEntity> builder)
    {
        // Table configuration
        builder.ToTable("Rule");

        // Primary key
        builder.HasKey(r => r.Id);

        // Property configurations
        builder.Property(r => r.FeatureId).IsRequired();
        builder.Property(r => r.Tenant).HasMaxLength(255);
        builder.Property(r => r.IsEnabled).IsRequired();

        // Relationships
        builder.HasOne(r => r.Environment)
            .WithMany(e => e.Rules)
            .HasForeignKey(r => r.EnvironmentId)
            .OnDelete(DeleteBehavior.SetNull); // Set null on environment deletion

        builder.HasOne(r => r.Feature)
            .WithMany(f => f.Rules)
            .HasForeignKey(r => r.FeatureId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade on feature deletion
    }
}