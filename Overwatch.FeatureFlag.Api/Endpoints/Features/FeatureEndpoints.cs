using MediatR;
using Overwatch.FeatureFlag.Api.Application.Handlers.Features;
using Overwatch.FeatureFlag.Interface.Models;

namespace Overwatch.FeatureFlag.Api.Endpoints.Features;

[ExcludeFromCodeCoverage]
public static class FeatureEndpoints
{
    public static WebApplication MapFeatureEndpoints(this WebApplication app)
    {
        // Get all features
        app.MapGet("/api/features", async ([FromServices] IMediator mediator) =>
                Results.Ok(await mediator.Send(new GetFeaturesRequest())))
            .WithTags("Features")
            .WithSummary("Lookup all Features");

        app.MapGet("/api/features/{id:guid}", async ([FromServices] IMediator mediator, [FromRoute] Guid id) =>
            Results.Ok(await mediator.Send(new GetFeatureByIdRequest(id))))
            .WithTags("Features")
            .WithSummary("Lookup a Feature by ID");

        app.MapGet("/api/features/{name}", async ([FromServices] IMediator mediator, [FromRoute] string name) =>
            Results.Ok(await mediator.Send(new GetFeatureByNameRequest(name))))
            .WithTags("Features")
            .WithSummary("Lookup a Feature by name");

        app.MapPost("/api/features/{name}", async ([FromServices] IMediator mediator, [FromBody] CreateFeatureRequest createFeatureRequest) => 
                Results.Ok((object?)await mediator.Send(createFeatureRequest)))
            .WithTags("Features")
            .WithSummary("Create a new Feature")
            .WithDescription("Creates a new feature with the specified name and optional environment IDs.");


        app.MapDelete("/api/features/{id:guid}", async (IMediator mediator, [FromRoute]Guid id) =>
                Results.Ok(await mediator.Send(new DeleteFeatureRequest(id))))
            .WithTags("Features")
            .WithSummary("Delete a Feature by its ID");

        return app;
    }
}