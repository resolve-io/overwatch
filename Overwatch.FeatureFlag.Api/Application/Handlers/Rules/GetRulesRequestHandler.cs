using Overwatch.FeatureFlag.Api.Persistence.Extensions;
using Overwatch.FeatureFlag.Interface.Models;

namespace Overwatch.FeatureFlag.Api.Application.Handlers.Rules;

public sealed record GetRulesRequest : IRequest<IEnumerable<Rule>>
{
    public static GetRulesRequest GetRules = new GetRulesRequest();
};
public class GetRulesRequestHandler(ComponentDbContext db) : IRequestHandler<GetRulesRequest, IEnumerable<Rule>>
{
    public async Task<IEnumerable<Rule>> Handle(GetRulesRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        // Fetch all rules for the specified feature
        var rules = await db.Rules
            .ProjectToRule()
            .ToListAsync(cancellationToken);

        // Return the list of rules
        return rules;
    }
}

public sealed record GetRulesByIdRequest(Guid Id) : IRequest<Rule>;

public class GetRulesByIdRequestHandler(ComponentDbContext db) : IRequestHandler<GetRulesByIdRequest, Rule>
{
    public async Task<Rule> Handle(GetRulesByIdRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        // Fetch the rule by ID
        var rule = await db.Rules
            .Where(r => r.Id == request.Id)
            .Include(r => r.Environment)
            .Include(r => r.Feature)
            .ProjectToRule()
            .FirstOrDefaultAsync(cancellationToken);

        if (rule == null)
            throw new KeyNotFoundException($"Rule with ID {request.Id} not found.");

        // Return the rule
        return rule;
    }
}
