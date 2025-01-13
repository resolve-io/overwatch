namespace Overwatch.FeatureFlag.Api.Application.Handlers.Environments;

public record UpdateEnvironmentNameRequest(Guid Id, string Name) : IRequest<bool>;
public class UpdateEnvironmentNameHandler(ComponentDbContext db) : IRequestHandler<UpdateEnvironmentNameRequest, bool>
{
    public async Task<bool> Handle(UpdateEnvironmentNameRequest request, CancellationToken cancellationToken)
    {
        // Retrieve the existing environment record
        var existingEnvironment = await db.Environments.FindAsync(new object[] { request.Id }, cancellationToken);

        if (existingEnvironment == null)
        {
            // Return false if no environment is found with the given ID
            return false;
        }

        // Create a new record using the existing one and update the name
        var updatedEnvironment = existingEnvironment with { Name = request.Name };

        // Update the tracked entity in the database
        db.Entry(existingEnvironment).CurrentValues.SetValues(updatedEnvironment);

        // Save the changes
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }
}