namespace Overwatch.FeatureFlag.Interface;

public record CreateRuleRequest([Required]Guid FeatureId, Guid? EnvironmentId, string Tenant, bool IsEnabled) : IRequest<Rule>;

public record CreateEnvironmentRequest([Required]string Name) : IRequest<Environment>;