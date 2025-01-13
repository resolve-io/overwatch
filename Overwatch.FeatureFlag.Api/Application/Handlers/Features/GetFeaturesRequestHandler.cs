using Overwatch.FeatureFlag.Api.Persistence.Extensions;
using Overwatch.FeatureFlag.Interface.Models;

namespace Overwatch.FeatureFlag.Api.Application.Handlers.Features;


public record GetFeaturesRequest : IRequest<List<Feature>>;
public class GetFeaturesHandler(ComponentDbContext db) : IRequestHandler<GetFeaturesRequest, List<Feature>>
{
    public async Task<List<Feature>> Handle(GetFeaturesRequest request, CancellationToken cancellationToken)
    {
        return await db.Features
            .Include(f => f.Rules) // Include rules for features
            .ThenInclude(r => r.Environment) // Include environment for rules
            .ProjectToFeature()
            .ToListAsync(cancellationToken);
    }
}