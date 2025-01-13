using Overwatch.FeatureFlag.Api.Application.Handlers.Features;
using Overwatch.FeatureFlag.Api.Application.Handlers.Rules;

namespace Overwatch.FeatureFlag.Api.Endpoints.Rules;

[ExcludeFromCodeCoverage]
public static class RulesEndpoints
{
    public static WebApplication MapRulesEndpoints(this WebApplication app)
    {
        app.MapGet("/api/features/rules", async (
                    [FromServices] IMediator mediator) =>
                        Results.Ok(await mediator.Send(GetRulesRequest.GetRules)))
            .WithTags("Features")
            .WithSummary("Get all rules for a specific feature");

        app.MapGet("/api/features/is-enabled/{name}/{environment}/{tenant}", async (
                    [FromServices] IMediator mediator, 
                    [FromRoute] string name, 
                    [FromRoute] string environment,
                    [FromRoute] string tenant) =>
                        Results.Ok(await mediator.Send(new IsFeatureEnabledRequest(name, environment, tenant))))
            .WithTags("Features")
            .WithSummary("Determine if a feature is enabled for a specific environment and tenant")
            .WithDescription(
                """
                Determines if a feature is enabled for a given environment and tenant.

                Example:
                GET /api/features/is-enabled/my-feature/development/my-tenant

                The logic for evaluating rules is as follows (note that the first one wins, regardless of whether it indicates the feature is enabled or disabled):
                  1. A rule specifying a feature is enabled/disabled for a specific environment and tenant.
                  2. A rule specifying a feature is enabled/disabled for _any_ environment and a _specific_ tenant.
                  3. A rule specifying a feature is enabled/disabled for a _specific_ environment and _any_ tenant.
                  4. A rule specifying a feature is enabled/disabled for _any_ environment and _any_ tenant.
                  5. If no relevant rules are found, the feature is disabled.
                """
            );

        app.MapPost("/api/features", async (
                    [FromServices] IMediator mediator, 
                    [FromBody] CreateRuleRequest request) =>
                        Results.Ok(await mediator.Send(request)))
            .WithTags("Features")
            .WithSummary("Create a new Rule")
            .WithDescription(
                """
                Adds or updates a rule for a feature.

                Example:
                POST /features/my-feature/rules
                {
                    "environment": "development",
                    "tenant": "*"
                }

                The logic for evaluating rules is as follows (note that the first one wins, regardless of whether it indicates the feature is enabled or disabled):
                  1. A rule specifying a feature is enabled/disabled for a specific environment and tenant.
                  2. A rule specifying a feature is enabled/disabled for _any_ environment and a _specific_ tenant.
                  3. A rule specifying a feature is enabled/disabled for a _specific_ environment and _any_ tenant.
                  4. A rule specifying a feature is enabled/disabled for _any_ environment and _any_ tenant.
                  5. If no relevant rules are found, the feature is disabled.

                When creating a rule, provide `"*"` as the value for tenant or environment to indicate the rule applies to _any_ tenant or environment, respectively. Specifying `null` or `undefined` will result in an HTTP 400 Bad Request.
                """
            );

        app.MapDelete("/api/features/rules/delete/{ruleId}", async (
                [FromServices] IMediator mediator, 
                [FromRoute] Guid ruleId) =>
                    Results.Ok(await mediator.Send(DeleteRuleRequest.CreateDeleteRuleRequest(ruleId))))
            .WithTags("Features")
            .WithSummary("Delete a Rule");

        app.MapGet("/api/features/{featureId}/rules", async (
                [FromServices] IMediator mediator, 
                [FromRoute] Guid featureId) =>
                    Results.Ok(await mediator.Send(new GetRulesForFeatureQuery(featureId))))
            .WithTags("Features")
            .WithSummary("Get all rules for a specific feature");

        app.MapPut("/api/features/rules/{ruleId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid ruleId,
                [FromBody] bool enabled) =>
                    Results.Ok(await mediator.Send(new UpdateRuleRequest(ruleId, enabled))))
            .WithTags("Features")
            .WithSummary("Update an existing rule");

        return app;
    }
}
