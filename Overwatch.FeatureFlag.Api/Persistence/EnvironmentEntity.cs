using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Overwatch.FeatureFlag.Api.Persistence;

[Table("Environment")] // Maps this class to the "Environment" table
public record EnvironmentEntity : EntityBase, IEntityTypeConfiguration<EnvironmentEntity>
{
    /// <summary>
    /// Gets or sets the name of the environment.
    /// </summary>
    [Required] // Ensures this field is not nullable
    [MaxLength(255)] // Sets a maximum length for the column
    public string Name { get; init; } = string.Empty; // Provide a default value

    /// <summary>
    /// Navigation property for associated rules.
    /// </summary>
    public IList<RuleEntity> Rules { get; init; } = new List<RuleEntity>();

    /// <summary>
    /// A wildcard constant used for rules applying to all tenants or environments.
    /// </summary>
    public static readonly string Wildcard = "*";

    /// <summary>
    /// Configures the entity using the EF model builder.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<EnvironmentEntity> builder)
    {
        // Table configuration
        builder.ToTable("Environment");

        // Primary key
        builder.HasKey(e => e.Id);

        // Property configurations
        builder.Property(e => e.Name)
            .IsRequired() // Marks as not nullable
            .HasMaxLength(255); // Limits string length

        // Relationships
        builder.HasMany(e => e.Rules)
            .WithOne(r => r.Environment)
            .HasForeignKey(r => r.EnvironmentId)
            .OnDelete(DeleteBehavior.Cascade); // Cascades on delete
    }
}