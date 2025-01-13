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