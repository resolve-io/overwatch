namespace Overwatch.FeatureFlag.Interface.Models;

public record CreateFeatureRequest(string Name, Guid[]? EnvironmentIds) : IRequest<Feature>;
public record Feature
{
    /// <summary>
    /// The unique identifier for the feature.
    /// </summary>
    [Description("The unique identifier for the feature.")]
    public Guid Id { get; init; }

    /// <summary>
    /// The date and time when the feature was created.
    /// </summary>
    [Description("The date and time when the feature was created.")]
    public DateTime DateCreated { get; init; }

    /// <summary>
    /// The date and time when the feature was last modified.
    /// </summary>
    [Description("The date and time when the feature was last modified.")]
    public DateTime DateModified { get; init; }

    /// <summary>
    /// The name of the feature.
    /// </summary>
    [Description("The name of the feature.")]
    [Required(ErrorMessage = "Feature name is required.")]
    [MaxLength(255, ErrorMessage = "Feature name cannot exceed 255 characters.")]
    public string Name { get; set; }

    /// <summary>
    /// Indicates if the feature is enabled.
    /// </summary>
    [Description("Indicates if the feature is enabled.")]
    public bool IsEnabled { get; set; }

    /// <summary>
    /// The list of rules associated with this feature.
    /// </summary>
    [Description("The list of rules associated with this feature.")]
    public IList<Rule> Rules { get; init; } = new List<Rule>();


}
