using Overwatch.FeatureFlag.Api.Persistence.Extensions;
using Overwatch.FeatureFlag.Interface.Models;

namespace Overwatch.FeatureFlag.Api.Application.Handlers.Features;

public class CreateFeaturesRequestHandler(ComponentDbContext db) : IRequestHandler<CreateFeatureRequest, Feature>
{
    public async Task<Feature> Handle(CreateFeatureRequest request, CancellationToken cancellationToken)
    {
        // Validate the name parameter
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Feature name must not be null or empty.", nameof(request.Name));
        }

        if (request.Name.Length > 255)
        {
            throw new ArgumentException("Feature name must not exceed 255 characters.", nameof(request.Name));
        }

        // Check if a feature with the same name already exists
        var existingFeature = await db.Features
            .FirstOrDefaultAsync(f => f.Name == request.Name, cancellationToken);

        if (existingFeature != null)
        {
            throw new InvalidOperationException($"A feature with the name '{request.Name}' already exists.");
        }

        // Create a new feature entity
        var feature = new FeatureEntity { Name = request.Name, };


        // Add and save the feature
        var entityEntry = db.Features.Add(feature);
        await db.SaveChangesAsync(cancellationToken);

        // Retrieve the created feature (ensure AsNoTracking for API projection)
        var createdFeature = await db.Features
            .Where(f => f.Id == entityEntry.Entity.Id)
            .ProjectToFeature()
            .FirstAsync(cancellationToken);

        return createdFeature;
    }
}