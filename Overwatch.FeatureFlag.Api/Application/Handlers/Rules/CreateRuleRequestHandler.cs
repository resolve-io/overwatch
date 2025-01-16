using Overwatch.FeatureFlag.Api.Persistence;
using Overwatch.FeatureFlag.Interface.Models;

namespace Overwatch.FeatureFlag.Api.Application.Handlers.Rules;

public class CreateRuleRequestHandler(
    ComponentDbContext dbContext,
    IMediator mediator) : IRequestHandler<CreateRuleRequest, Rule>
{
    public async Task<Rule> Handle(CreateRuleRequest request, CancellationToken cancellationToken)
    {
        // Validate the feature
        var feature = await dbContext.Features
            .FirstOrDefaultAsync(f => f.Id == request.FeatureId, cancellationToken);
        if (feature == null)
            throw new KeyNotFoundException("Feature not found.");

        // Validate the environment if provided
        if (request.EnvironmentId.HasValue)
        {
            var environment = await dbContext.Environments
                .FirstOrDefaultAsync(e => e.Id == request.EnvironmentId.Value, cancellationToken);
            if (environment == null)
                throw new KeyNotFoundException("Environment not found.");
        }

        // Check for an existing rule with the same combination
        var existingRule = await dbContext.Rules.FirstOrDefaultAsync(r =>
            r.EnvironmentId == request.EnvironmentId &&
            r.Tenant == request.Tenant &&
            r.FeatureId == request.FeatureId,
            cancellationToken);

        RuleEntity ruleEntity;

        if (existingRule == null)
        {
            // Create a new rule
            ruleEntity = new RuleEntity
            {
                FeatureId = feature.Id,
                EnvironmentId = request.EnvironmentId,
                Tenant = request.Tenant,
                IsEnabled = request.IsEnabled,
            };

            // Add the new rule to the database
            dbContext.Rules.Add(ruleEntity);
        }
        else
        {
            // Update the existing rule using `with` syntax for records
            ruleEntity = existingRule with
            {
                IsEnabled = request.IsEnabled
            };

            // Update the tracked entity in the DbContext
            dbContext.Entry(existingRule).CurrentValues.SetValues(ruleEntity);
        }

        // Save changes to the database
        await dbContext.SaveChangesAsync(cancellationToken);

        // Retrieve the rule by ID to return a fully populated entity
        return await mediator.Send(new GetRulesByIdRequest(ruleEntity.Id), cancellationToken);
    }


}