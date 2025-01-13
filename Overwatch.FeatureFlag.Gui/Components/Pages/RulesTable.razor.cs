using Microsoft.Extensions.Hosting;
using MudBlazor;
using Overwatch.FeatureFlag.Interface;

namespace Overwatch.FeatureFlag.Gui.Components.Pages;

public partial class RulesTable : ComponentBase
{
    private string searchString = "";

    private Rule? selectedRule;
    private List<Rule> rules = new();
    private bool isLoading = true;
    private List<Rule> filteredRules => rules.Where(FilterFunc).ToList();
    private Rule ruleBeforeEdit;

    [Inject] private RulesApiClient RulesApiClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            rules = (await RulesApiClient.GetAllRulesAsync()).ToList();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading rules: {ex.Message}", Severity.Error);
        }
    }

    private void CommitRule(object rule)
    {
        if (rule is Rule castedRule)
        {
            Task.Run(async () =>
            {
                try
                {
                    await RulesApiClient.UpdateRuleAsync(castedRule.Id, castedRule.IsEnabled);
                    Snackbar.Add($"Rule '{castedRule.FeatureName}' updated successfully.", Severity.Success);
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error updating rule '{castedRule.FeatureName}': {ex.Message}", Severity.Error);
                }
            });
        }
        else
        {
            Snackbar.Add("Invalid rule type. Unable to update.", Severity.Error);
        }
    }

    private void BackupRule(object rule)
    {
        if (rule is Rule castedRule)
        {
            ruleBeforeEdit = new Rule
            {
                Id = castedRule.Id,
                FeatureId = castedRule.FeatureId,
                EnvironmentId = castedRule.EnvironmentId,
                Tenant = castedRule.Tenant,
                IsEnabled = castedRule.IsEnabled,
                DateCreated = castedRule.DateCreated,
                DateModified = castedRule.DateModified,
                EnvironmentName = castedRule.EnvironmentName,
                FeatureName = castedRule.FeatureName
            };
        }
        else
        {
            throw new InvalidCastException("The provided object is not of type Rule.");
        }
    }

    private void ResetRule(object rule)
    {
        if (rule is Rule castedRule)
        {
            castedRule.IsEnabled = ruleBeforeEdit.IsEnabled;
        }
        else
        {
            throw new InvalidCastException("The provided object is not of type Rule.");
        }
    }

    private void AddNewRule()
    {
        Snackbar.Add("Add new rule clicked.", Severity.Info);
    }

    private bool FilterFunc(Rule rule)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        return rule.FeatureName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
               rule.EnvironmentName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
               rule.Tenant.Contains(searchString, StringComparison.OrdinalIgnoreCase);
    }

    private async Task RefreshRulesList()
    {
        isLoading = true; // Start loading
        try
        {
            rules = (await RulesApiClient.GetAllRulesAsync())?.ToList() ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error fetching environments: {ex.Message}", Severity.Error);
        }
        finally
        {
            isLoading = false; // Stop loading
            await InvokeAsync(StateHasChanged); // Notify UI
        }
    }
}
