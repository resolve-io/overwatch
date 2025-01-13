using MudBlazor.Services;
using Overwatch.FeatureFlag.Gui;
using Overwatch.FeatureFlag.Gui.Components;
using System.Net.Sockets;
using Overwatch.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();



builder.Services.AddHttpClient<WeatherApiClient>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new Uri("https+http://featureflag-api");
    });
builder.Services.AddHttpClient<FeaturesApiClient>(client =>
{
    // Configure the base address for the Features API
    client.BaseAddress = new Uri("https+http://featureflag-api");
});
builder.Services.AddHttpClient<EnvironmentsApiClient>(client =>
{
    // Configure the base address for the Features API
    client.BaseAddress = new Uri("https+http://featureflag-api");
});
builder.Services.AddHttpClient<RulesApiClient>(client =>
{
    // Configure the base address for the Features API
    client.BaseAddress = new Uri("https+http://featureflag-api");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
