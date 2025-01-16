using Overwatch.FeatureFlag.Interface;

namespace Overwatch.FeatureFlag.Gui;

public class FeaturesApiClient(HttpClient httpClient)
{
    // GET: /api/features - Lookup all Features
    public async Task<IEnumerable<Feature>> GetAllFeaturesAsync()
    {
        var response = await httpClient.GetAsync("/api/features");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<Feature>>() ?? new List<Feature>();
    }

    // POST: /api/features - Create a new Rule
    public async Task CreateRuleAsync(CreateRuleRequest ruleRequest)
    {
        var response = await httpClient.PostAsJsonAsync("/api/features", ruleRequest);
        response.EnsureSuccessStatusCode();
    }

    // GET: /api/features/{id} - Lookup a Feature by ID
    public async Task<Feature?> GetFeatureByIdAsync(Guid id)
    {
        var response = await httpClient.GetAsync($"/api/features/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Feature>();
    }

    // DELETE: /api/features/{id} - Delete a Feature by ID
    public async Task DeleteFeatureByIdAsync(Guid id)
    {
        var response = await httpClient.DeleteAsync($"/api/features/{id}");
        response.EnsureSuccessStatusCode();
    }

    // POST: /api/features/{name} - Create a new Feature
    public async Task<Feature> CreateFeatureAsync(string name, Guid[]? environmentIds = null)
    {
        var body = new CreateFeatureRequest(name, environmentIds ?? []);

        var response = await httpClient.PostAsJsonAsync($"/api/features/{name}", body);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Feature>();
    }


    // GET: /api/features/{name} - Lookup a Feature by Name
    public async Task<Feature?> GetFeatureByNameAsync(string name)
    {
        var response = await httpClient.GetAsync($"/api/features/{name}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Feature>();
    }

    // GET: /api/features/is-enabled/{name}/{environment}/{tenant} - Check if Feature is Enabled
    public async Task<bool> IsFeatureEnabledAsync(string name, string environment, string tenant)
    {
        var response = await httpClient.GetAsync($"/api/features/is-enabled/{name}/{environment}/{tenant}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<bool>();
    }

    // DELETE: /api/features/rules/delete/{ruleId} - Delete a Rule
    public async Task DeleteRuleAsync(Guid ruleId)
    {
        var response = await httpClient.DeleteAsync($"/api/features/rules/delete/{ruleId}");
        response.EnsureSuccessStatusCode();
    }
}