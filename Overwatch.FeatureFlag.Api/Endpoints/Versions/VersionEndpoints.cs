namespace Overwatch.FeatureFlag.Api.Endpoints.Versions;

[ExcludeFromCodeCoverage]
public static class VersionEndpoints
{
    public record GetVersionQuery : IRequest<string>;
    public static WebApplication MapVersionEndpoints(this WebApplication app)
    {
        _ = app.MapGet("/api/version", async ([FromServices] IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetVersionQuery())))
            .WithSummary("Get the application version details")
            .WithDescription("This endpoint retrieves detailed information about the application's version.")
            .WithTags("ApplicationVersion");
            

        return app;
    }
}