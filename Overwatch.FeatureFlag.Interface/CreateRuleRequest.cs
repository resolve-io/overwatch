namespace Overwatch.FeatureFlag.Interface;

public record CreateRuleRequest([Required] string Environment, [Required] string Tenant, [Required] string Name, [Required] bool IsEnabled) : IRequest<Rule>;

public record CreateEnvironmentRequest([Required]string Name) : IRequest<Environment>;