namespace Overwatch.FeatureFlag.Api.Application.Handlers.Environments;

public class CreateEnvironmentHandler(ComponentDbContext db) : IRequestHandler<CreateEnvironmentRequest, Environment>
{
    public async Task<Environment> Handle(CreateEnvironmentRequest request, CancellationToken cancellationToken)
    {
        // Create the environment entity
        var environmentEntity = new EnvironmentEntity
        {
            Name = request.Name
        };

        // Add the entity to the database
        db.Environments.Add(environmentEntity);
        await db.SaveChangesAsync(cancellationToken);

        // Project the entity to the domain model
        return new Environment
        {
            Id = environmentEntity.Id,
            Name = environmentEntity.Name,
            DateCreated = environmentEntity.DateCreated,
            DateModified = environmentEntity.DateModified
        };
    }
}