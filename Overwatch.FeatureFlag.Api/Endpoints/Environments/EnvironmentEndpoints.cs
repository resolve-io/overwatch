namespace Overwatch.FeatureFlag.Api.Endpoints.Environments;

[ExcludeFromCodeCoverage]
public static class EnvironmentEndpoints
{
    public static WebApplication MapEnvironmentEndpoints(this WebApplication app)
    {
        app.MapGet("/api/environments", async ([FromServices] IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetEnvironmentsQuery())))
            .WithTags("Environments");

        app.MapPost("/api/environments/{name}", async ([FromServices] IMediator mediator, [FromRoute] string name) =>
            Results.Ok(await mediator.Send(new CreateEnvironmentRequest(name))))
            .WithSummary("Creates an Environment")
            .WithTags("Environments");

        app.MapPut("/api/environments/{id}/name", async ([FromServices] IMediator mediator, [FromRoute] Guid id, [FromBody] string newName) =>
            Results.Ok(await mediator.Send(new UpdateEnvironmentNameRequest(id, newName))))
            .WithSummary("Updates the name of an Environment")
            .WithTags("Environments");

        return app;
    }
}
