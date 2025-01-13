using Overwatch.FeatureFlag.Api.Endpoints.Rules;

namespace Overwatch.FeatureFlag.Api.Endpoints;

public static class EndpointRegister
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        app.MapVersionEndpoints();
        app.MapFeatureEndpoints();
        app.MapEnvironmentEndpoints();
        app.MapRulesEndpoints();

        return app;
    }
}