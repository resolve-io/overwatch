using Aspire.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var sqlConnection = builder.AddConnectionString("mssqlConnection");
//var database = builder.AddSqlServer("mssqlServer").AddDatabase("overwatch", "overwatch")
//    .WithConnectionStringRedirection(sqlConnection.Resource);

//var mysql = builder
//    .AddSqlServer("sqlserver")
//    .WithLifetime(ContainerLifetime.Persistent)
//    .AddDatabase("db");

var migrationService = builder.AddProject<Overwatch_FeatureFlag_MigrationService>("featureflag-migration")
    .WithReference(sqlConnection);
    

var apiService = builder.AddProject<Overwatch_FeatureFlag_Api>("featureflag-api")
    .WithExternalHttpEndpoints()
    .WaitForCompletion(migrationService)
    .WithReference(sqlConnection)
    .WithReference(cache)
    .WaitFor(cache);

builder.AddProject<Overwatch_FeatureFlag_Gui>("featureflag-gui")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService)
    .WaitFor(cache)
    .WaitFor(apiService);

builder.Build().Run();


