namespace Overwatch.FeatureFlag.Gui;

public class EnvironmentsApiClient(HttpClient httpClient)
{
    // GET: /api/environments - Lookup all Environments
    public async Task<IEnumerable<Environment>> GetAllEnvironmentsAsync()
    {
        var response = await httpClient.GetAsync("/api/environments");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<Environment>>() ?? new List<Environment>();
    }

    // POST: /api/environments/{name} - Create a new Environment
    public async Task<Environment> CreateEnvironmentAsync(string name)
    {
        var response = await httpClient.PostAsync($"/api/environments/{name}", null);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Environment>()
               ?? throw new InvalidOperationException("Failed to create the environment.");
    }

    // PUT: /api/environments/{id}/name - Update the name of an Environment
    public async Task UpdateEnvironmentAsync(Environment environment)
    {
        // Construct the request body
        var content = JsonContent.Create(environment.Name);

        // Send the PUT request to the API
        var response = await httpClient.PutAsync($"/api/environments/{environment.Id}/name", content);

        // Ensure the response indicates success
        response.EnsureSuccessStatusCode();
    }
}