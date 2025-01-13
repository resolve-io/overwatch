namespace Overwatch.FeatureFlag.Api.Application.Handlers.Features;

public record UpdateFeatureRequest(Guid Id, string Name) : IRequest<bool>;
public class UpdateFeatureRequestHandler(ComponentDbContext db) : IRequestHandler<UpdateFeatureRequest, bool>
{
    public async Task<bool> Handle(UpdateFeatureRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate the request parameters
            if (request.Id == Guid.Empty)
            {
                throw new ArgumentException("Feature ID must be a valid GUID.", nameof(request.Id));
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Feature name must not be null or empty.", nameof(request.Name));
            }

            if (request.Name.Length > 255)
            {
                throw new ArgumentException("Feature name must not exceed 255 characters.", nameof(request.Name));
            }

            // Find the feature in the database
            var feature = await db.Features.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
            if (feature == null)
                throw new NotFoundException($"The Feature with the supplied id was not found.");

            // Update the feature
            // Create a new feature record with updated name
            var updatedFeature = feature! with
            {
                Name = request.Name,
            };

            // Update the feature
            db.Entry(feature).CurrentValues.SetValues(updatedFeature);


            // Save the changes
            await db.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            // Log the exception if needed
            // logger.LogError(ex, "Error updating feature with ID {FeatureId}", request.Id);

            return false;
        }
    }
}