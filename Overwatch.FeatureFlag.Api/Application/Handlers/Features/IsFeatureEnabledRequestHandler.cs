namespace Overwatch.FeatureFlag.Api.Application.Handlers.Features;

public record IsFeatureEnabledRequest(string Feature, string Environment, string Tenant) : IRequest<bool>;
public class IsFeatureEnabledRequestHandler(ComponentDbContext db) : IRequestHandler<IsFeatureEnabledRequest, bool>
{
    public async Task<bool> Handle(IsFeatureEnabledRequest request, CancellationToken cancellationToken)
    {
        // Validate the feature by name
        var feature = await db.Features
            .FirstOrDefaultAsync(f => f.Name == request.Feature, cancellationToken);
        if (feature == null)
            throw new NotFoundException("The Feature with the supplied name was not found.");

        // Validate the environment by name (if not wildcard)
        Guid? environmentId = null;
        if (request.Environment != EnvironmentEntity.Wildcard)
        {
            var environment = await db.Environments
                .FirstOrDefaultAsync(e => e.Name == request.Environment, cancellationToken);
            if (environment == null)
                throw new NotFoundException("The Environment with the supplied name was not found.");

            environmentId = environment.Id;
        }

        // Prepare the tenant value or null if wildcard
        var tenant = request.Tenant != EnvironmentEntity.Wildcard ? request.Tenant : null;

        /*
         * Query rules that might apply based on the environment and tenant.
         * The query eliminates all rules that definitely do not apply.
         */
        var rules = await db.Rules
            .Where(r =>
                r.FeatureId == feature.Id &&
                (r.EnvironmentId == environmentId || r.EnvironmentId == null) &&
                (r.Tenant == tenant || r.Tenant == null || r.Tenant == EnvironmentEntity.Wildcard))
            .ToListAsync(cancellationToken);

        RuleEntity? matchingRule = null;

        // Prioritization logic for rules

        // 1. Rules for THIS environment and THIS tenant
        matchingRule = rules.SingleOrDefault(r => r.EnvironmentId == environmentId && r.Tenant == tenant);
        if (matchingRule != null)
            return matchingRule.IsEnabled;

        // 2. Rules for ANY environment and THIS tenant
        matchingRule = rules.SingleOrDefault(r => r.EnvironmentId == null && r.Tenant == tenant);
        if (matchingRule != null)
            return matchingRule.IsEnabled;

        // 3. Rules for THIS environment and ANY tenant
        matchingRule = rules.SingleOrDefault(r => r.EnvironmentId == environmentId && (r.Tenant == null || r.Tenant == EnvironmentEntity.Wildcard));
        if (matchingRule != null)
            return matchingRule.IsEnabled;

        // 4. Rules for ANY environment and ANY tenant
        matchingRule = rules.SingleOrDefault(r => r.EnvironmentId == null && (r.Tenant == null || r.Tenant == EnvironmentEntity.Wildcard));
        if (matchingRule != null)
            return matchingRule.IsEnabled;

        // 5. Fallback: It's not enabled
        return false;
    }

}