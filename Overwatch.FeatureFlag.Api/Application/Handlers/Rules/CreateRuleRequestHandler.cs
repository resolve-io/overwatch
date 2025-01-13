using Overwatch.FeatureFlag.Interface.Models;

namespace Overwatch.FeatureFlag.Api.Application.Handlers.Rules;

public class CreateRuleRequestHandler(ComponentDbContext dbContext) : IRequestHandler<CreateRuleRequest, Rule>
{
    public async Task<Rule> Handle(CreateRuleRequest request, CancellationToken cancellationToken)
    {
        // Validate the feature by name
        var feature = await dbContext.Features
            .FirstOrDefaultAsync(f => f.Name == request.Name, cancellationToken);

        if (feature == null)
            throw new KeyNotFoundException($"Feature with name '{request.Name}' not found.");

        // Validate the environment if not wildcard
        Guid? environmentId = null;
        if (request.Environment != Rule.Wildcard)
        {
            var environment = await dbContext.Environments
                .FirstOrDefaultAsync(e => e.Name == request.Environment, cancellationToken);

            if (environment == null)
                throw new KeyNotFoundException($"Environment with name '{request.Environment}' not found.");

            environmentId = environment.Id;
        }

        // Prepare the tenant ID or null if wildcard
        var tenant = request.Tenant != Rule.Wildcard ? request.Tenant : null;

        // Attempt to find an existing rule with the same combination
        var existingRule = await dbContext.Rules
            .FirstOrDefaultAsync(
                r => r.EnvironmentId == environmentId &&
                     r.Tenant == tenant &&
                     r.FeatureId == feature.Id,
                cancellationToken);

        // Create or update the rule
        var rule = existingRule == null
            ? new RuleEntity
            {
                Id = Guid.NewGuid(),
                FeatureId = feature.Id,
                EnvironmentId = environmentId,
                Tenant = tenant,
                IsEnabled = request.IsEnabled,
            }
            : existingRule with
            {
                IsEnabled = request.IsEnabled,
            };

        // Add or update the rule in the DbContext
        if (existingRule == null)
        {
            dbContext.Rules.Add(rule);
        }
        else
        {
            dbContext.Entry(existingRule).CurrentValues.SetValues(rule);
        }

        // Save changes
        await dbContext.SaveChangesAsync(cancellationToken);

        return null;

        // Project the saved rule into the Rule model
        //return await dbContext.Rules
        //    .Where(r => r.Id == rule.Id)
        //    .Include(r => r.Environment)
        //    .Include(r => r.Feature)
        //    .Select(r => new Rule
        //    {
        //        Id = r.Id,
        //        Tenant = r.Tenant,
        //        IsEnabled = r.IsEnabled,
        //        DateCreated = r.DateCreated,
        //        DateModified = r.DateModified,
        //        Environment = r.Environment == null ? null : new Environment
        //        {
        //            Id = r.Environment.Id,
        //            Name = r.Environment.Name,
        //            DateCreated = r.Environment.DateCreated,
        //            DateModified = r.Environment.DateModified
        //        },
        //        Feature = r.Feature == null ? null : new Feature
        //        {
        //            Id = r.Feature.Id,
        //            Name = r.Feature.Name,
        //            DateCreated = r.Feature.DateCreated,
        //            DateModified = r.Feature.DateModified
        //        }
        //    })
        //    .FirstAsync(cancellationToken);
    }
}