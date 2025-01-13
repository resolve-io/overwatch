namespace Overwatch.FeatureFlag.Interface.Models;

public record Rule
{
    /// <summary>
    /// A wildcard constant used for rules applying to all tenants or environments.
    /// </summary>
    public static readonly string Wildcard = "*";

    /// <summary>
    /// The unique identifier for the rule.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The unique identifier of the feature this rule applies to.
    /// </summary>
    public Guid FeatureId { get; init; }

    /// <summary>
    /// The unique identifier of the environment this rule applies to, or NULL for all environments.
    /// </summary>
    public Guid? EnvironmentId { get; init; }

    /// <summary>
    /// The name of the tenant this rule applies to, or a wildcard (*) for all tenants.
    /// </summary>
    public string Tenant { get; init; } = Wildcard;

    /// <summary>
    /// Indicates whether the feature is enabled for this rule.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// The date and time when this rule was created.
    /// </summary>
    public DateTime DateCreated { get; init; }

    /// <summary>
    /// The date and time when this rule was last modified.
    /// </summary>
    public DateTime DateModified { get; init; }

    /// <summary>
    /// The name of the environment this rule applies to, or a wildcard (*) for all environments.
    /// </summary>
    public string EnvironmentName { get; init; } = Wildcard;

    /// <summary>
    /// The name of the feature this rule applies to.
    /// </summary>
    public string FeatureName { get; init; } = Wildcard;
}