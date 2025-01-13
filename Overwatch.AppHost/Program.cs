using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var mysql = builder.AddSqlServer("sqlserver")
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("db");

var migrationService = builder.AddProject<Overwatch_FeatureFlag_MigrationService>("featureflag-migration")
    .WithReference(mysql)
    .WaitFor(mysql);

var apiService = builder.AddProject<Overwatch_FeatureFlag_Api>("featureflag-api")
    .WithExternalHttpEndpoints()
    .WaitForCompletion(migrationService)
    .WithReference(mysql)
    .WithReference(cache)
    .WaitFor(cache)
    .WaitFor(mysql);

builder.AddProject<Overwatch_FeatureFlag_Gui>("featureflag-gui")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService)
    .WaitFor(cache)
    .WaitFor(apiService);

builder.Build().Run();


