using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Overwatch.FeatureFlag.Api.Application;
using Overwatch.FeatureFlag.Api.Endpoints;
using Overwatch.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.RegisterApplication(builder.Configuration);

builder.AddSqlServerDbContext<ComponentDbContext>(connectionName: "db");

//builder.Services.AddDbContextPool<ComponentDbContext>(options =>
//    options.UseSqlServer("db", sqlOptions =>
//    {
//        // Workaround for https://github.com/dotnet/aspire/issues/1023
//        sqlOptions.ExecutionStrategy(c => new RetryingSqlServerRetryingExecutionStrategy(c));
//    }));
//builder.EnrichSqlServerDbContext<ComponentDbContext>(settings =>
//    // Disable Aspire default retries as we're using a custom execution strategy
//    settings.DisableRetry = true);

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("swagger/v1/swagger.json");
    app.UseSwaggerUI();
}

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.RegisterEndpoints();

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

[ExcludeFromCodeCoverage]
public partial class Program { }

public class RetryingSqlServerRetryingExecutionStrategy(ExecutionStrategyDependencies dependencies) : SqlServerRetryingExecutionStrategy(dependencies)
{
    protected override bool ShouldRetryOn(Exception exception)
    {
        if (exception is SqlException sqlException)
        {
            foreach (SqlError error in sqlException.Errors)
            {
                // EF Core issue logged to consider making this a default https://github.com/dotnet/efcore/issues/33191
                if (error.Number is 4060)
                {
                    // Don't retry on login failures associated with default database not existing due to EF migrations not running yet
                    return false;
                }
                // Workaround for https://github.com/dotnet/aspire/issues/1023
                else if (error.Number is 0 || (error.Number is 203 && sqlException.InnerException is Win32Exception))
                {
                    return true;
                }
            }
        }

        return base.ShouldRetryOn(exception);
    }
}