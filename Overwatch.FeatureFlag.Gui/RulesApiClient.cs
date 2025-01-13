using Overwatch.FeatureFlag.Interface;

namespace Overwatch.FeatureFlag.Gui;

public class RulesApiClient(HttpClient httpClient)
{
    public async Task<IEnumerable<Rule>> GetAllRulesAsync()
    {
        var response = await httpClient.GetAsync($"/api/features/rules");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<Rule>>() ?? new List<Rule>();
    }

    // GET: /api/features/{featureId}/rules - Lookup all Rules for a specific feature
    public async Task<IEnumerable<Rule>> GetRulesForFeatureAsync(Guid featureId)
    {
        var response = await httpClient.GetAsync($"/api/features/{featureId}/rules");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<Rule>>() ?? new List<Rule>();
    }

    // POST: /api/features - Create a new Rule
    public async Task<Rule> CreateRuleAsync(CreateRuleRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("/api/features", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Rule>()
               ?? throw new InvalidOperationException("Failed to create the rule.");
    }

    // PUT: /api/features/rules/{ruleId} - Update an existing Rule
    public async Task UpdateRuleAsync(Guid ruleId, bool enabled)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/features/rules/{ruleId}", enabled);
        response.EnsureSuccessStatusCode();
    }

    // DELETE: /api/features/rules/delete/{ruleId} - Delete a Rule
    public async Task DeleteRuleAsync(Guid ruleId)
    {
        var response = await httpClient.DeleteAsync($"/api/features/rules/delete/{ruleId}");
        response.EnsureSuccessStatusCode();
    }

    // GET: /api/features/is-enabled/{name}/{environment}/{tenant} - Check if a feature is enabled
    public async Task<bool> IsFeatureEnabledAsync(string name, string environment, string tenant)
    {
        var response = await httpClient.GetAsync($"/api/features/is-enabled/{name}/{environment}/{tenant}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<bool>();
    }


}
