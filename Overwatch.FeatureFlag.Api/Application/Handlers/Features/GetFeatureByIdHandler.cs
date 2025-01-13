using Overwatch.FeatureFlag.Api.Persistence.Extensions;
using Overwatch.FeatureFlag.Interface.Models;

namespace Overwatch.FeatureFlag.Api.Application.Handlers.Features;

public record GetFeatureByIdRequest(Guid Id) : IRequest<List<Feature>>;
public class GetFeatureByIdHandler(ComponentDbContext db) : IRequestHandler<GetFeatureByIdRequest, List<Feature>>
{
    public async Task<List<Feature>> Handle(GetFeatureByIdRequest request, CancellationToken cancellationToken)
    {
        return await db.Features
            .Where(r => r.Id == request.Id)
            .Include(f => f.Rules) // Include rules for features
            .ThenInclude(r => r.Environment) // Include environment for rules
            .ProjectToFeature()
            .ToListAsync(cancellationToken);
    }

}