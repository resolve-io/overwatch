namespace Overwatch.FeatureFlag.Interface.Models;

public record Environment
{
    [Required]
    [Description("The unique identifier of the environment.")]
    public Guid Id { get; init; }

    [Description("The date and time the environment was created.")]
    public DateTime DateCreated { get; init; }

    [Description("The date and time the environment was last modified.")]
    public DateTime DateModified { get; init; }

    [Required]
    [MaxLength(100)]
    [Description("The name of the environment, e.g., 'Development' or 'Production'.")]
    public string Name { get; set; }

    [Description("The list of rules associated with the environment.")]
    public IList<Rule> Rules { get; init; } = new List<Rule>();
}