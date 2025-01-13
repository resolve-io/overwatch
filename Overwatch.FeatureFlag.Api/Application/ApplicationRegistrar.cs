namespace Overwatch.FeatureFlag.Api.Application;

public static class ApplicationRegistrar
{
    public static IServiceCollection RegisterApplication(this WebApplicationBuilder builder, IConfiguration config)
    {
        builder.Services.AddOptions();

        builder.Services.AddHttpContextAccessor();
        
        builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        
        return builder.Services;
    }
}