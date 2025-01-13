namespace Overwatch.FeatureFlag.Api.Application.Handlers.Features;

public record DeleteFeatureRequest(Guid Id) : IRequest<Unit>;
public class DeleteFeatureRequestHandler(ComponentDbContext db) : IRequestHandler<DeleteFeatureRequest, Unit>
{
    public async Task<Unit> Handle(DeleteFeatureRequest request, CancellationToken cancellationToken)
    {
        // Validate the FeatureId
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Feature ID must be a valid GUID.", nameof(request.Id));
        }

        // Find the feature by ID
        var feature = await db.Features
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (feature == null)
        {
            throw new KeyNotFoundException($"Feature with ID '{request.Id}' was not found.");
        }

        // Remove the feature
        db.Features.Remove(feature);
        await db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}