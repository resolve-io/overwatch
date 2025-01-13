namespace Overwatch.FeatureFlag.Api.Application.Handlers.Environments;

public record GetEnvironmentsQuery : IRequest<IList<Environment>>;
public class GetEnvironmentsHandler(ComponentDbContext db) : IRequestHandler<GetEnvironmentsQuery, IList<Environment>>
{
    public async Task<IList<Environment>> Handle(GetEnvironmentsQuery request, CancellationToken cancellationToken)
    {
        return await db.Environments.Select(x => new Environment
        {
            DateCreated = x.DateCreated,
            DateModified = x.DateModified,
            Id = x.Id,
            Name = x.Name,
        }).ToListAsync(cancellationToken: cancellationToken);
    }
}