using Microsoft.EntityFrameworkCore;
using Overwatch.FeatureFlag.Api.Persistence;
using Overwatch.FeatureFlag.MigrationService;
using Overwatch.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<DatabaseInitializer>();

builder.AddServiceDefaults();

builder.Services.AddDbContextPool<ComponentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("db"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("Overwatch.FeatureFlag.MigrationService");
        // Workaround for https://github.com/dotnet/aspire/issues/1023
        sqlOptions.ExecutionStrategy(c => new Overwatch.FeatureFlag.MigrationService.RetryingSqlServerRetryingExecutionStrategy(c));
    }));
builder.EnrichSqlServerDbContext<ComponentDbContext>(settings =>
    // Disable Aspire default retries as we're using a custom execution strategy
    settings.DisableRetry = true);

var app = builder.Build();

app.Run();