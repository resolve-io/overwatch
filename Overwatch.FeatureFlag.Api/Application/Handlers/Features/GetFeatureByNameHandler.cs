using Overwatch.FeatureFlag.Api.Persistence.Extensions;
using Overwatch.FeatureFlag.Interface.Models;

namespace Overwatch.FeatureFlag.Api.Application.Handlers.Features;


public record GetFeatureByNameRequest(string name) : IRequest<List<Feature>>;
public class GetFeatureByNameHandler(ComponentDbContext db) : IRequestHandler<GetFeatureByNameRequest, List<Feature>>
{
    public async Task<List<Feature>> Handle(GetFeatureByNameRequest request, CancellationToken cancellationToken)
    {
        return await db.Features
            .Where(r => r.Name == request.name)
            .Include(f => f.Rules) // Include rules for features
            .ThenInclude(r => r.Environment) // Include environment for rules
            .ProjectToFeature()
            .ToListAsync(cancellationToken);
    }

}