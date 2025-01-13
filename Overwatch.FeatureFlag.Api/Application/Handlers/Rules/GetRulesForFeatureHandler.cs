using Overwatch.FeatureFlag.Api.Persistence.Extensions;
using Overwatch.FeatureFlag.Interface.Models;

namespace Overwatch.FeatureFlag.Api.Application.Handlers.Rules;

public record GetRulesForFeatureQuery(Guid FeatureId) : IRequest<IEnumerable<Rule>>;
public class GetRulesForFeatureHandler(ComponentDbContext db) : IRequestHandler<GetRulesForFeatureQuery, IEnumerable<Rule>>
{
    public async Task<IEnumerable<Rule>> Handle(GetRulesForFeatureQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        // Fetch all rules for the specified feature
        var rules = await db.Rules
            .Where(r => r.FeatureId == request.FeatureId)
            .ProjectToRule()
            .ToListAsync(cancellationToken);

        // Return the list of rules
        return rules;
    }
}