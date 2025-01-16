using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace Overwatch.E2E.Tests;

[TestClass]
public class TestBase
{
    protected string _url = string.Empty;
    protected IBrowser Browser;
    protected IPage Page;

    [TestInitialize]
    public async Task InitializeAsync()
    {
        // Create the distributed application host
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Overwatch_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        // Retrieve and configure the base URL
        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        await resourceNotificationService.WaitForResourceAsync("featureflag-gui", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var webResource = appHost.Resources.FirstOrDefault(r => r.Name == "featureflag-gui");

        // get the 'http' endpoint for the resource
        var endpoint = webResource?.Annotations.OfType<EndpointAnnotation>().Where(x => x.Name == "http").FirstOrDefault();
        _url = endpoint.AllocatedEndpoint.UriString;

        //_url = httpClient.BaseAddress.ToString();

        // Initialize Playwright and launch a browser instance
        var playwright = await Playwright.CreateAsync();
        Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {

            Headless = false, // Optional: Run in headless mode
        });

        var context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            BaseURL = _url
        });

        Page = await context.NewPageAsync();
    }

    [TestCleanup]
    public async Task DisposeAsync()
    {
        // Ensure browser and page instances are properly disposed
        if (Page != null)
        {
            await Page.CloseAsync();
        }

        if (Browser != null)
        {
            await Browser.CloseAsync();
        }
    }

    protected async Task GetHomePageAsync()
    {
        // Navigate to the homepage and verify the title
        if (!string.IsNullOrEmpty(_url))
        {
            await Page.GotoAsync("/"); // Navigate to the root URL using the configured BaseURL
            var title = await Page.TitleAsync();
            Assert.AreEqual("Home", title);
        }
    }
}

[TestClass]
public class WebTests : TestBase
{
    [TestMethod]
    public async Task HomePageIsDisplayed()
    {
        await GetHomePageAsync();
    }
}
