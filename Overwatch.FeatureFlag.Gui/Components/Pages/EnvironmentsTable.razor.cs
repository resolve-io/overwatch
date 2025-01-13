using MudBlazor;
using Overwatch.FeatureFlag.Gui.Components.Dialogs;

namespace Overwatch.FeatureFlag.Gui.Components.Pages;

public partial class EnvironmentsTable : ComponentBase
{
    private string searchString = "";
    private Environment? selectedEnvironment;
    private List<Environment> environments = new List<Environment>();

    private Environment environmentBeforeEdit = new();
    private bool isLoading = true;

    [Inject] private EnvironmentsApiClient EnvironmentsApiClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await RefreshEnvironmentList();
    }

    private void CommitEnvironment(object environment)
    {
        if (environment is Environment castedEnvironment)
        {
            Task.Run(async () =>
            {
                try
                {
                    await EnvironmentsApiClient.UpdateEnvironmentAsync(castedEnvironment);
                    Snackbar.Add($"Environment '{castedEnvironment.Name}' updated successfully.", Severity.Success);
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error updating environment '{castedEnvironment.Name}': {ex.Message}", Severity.Error);
                }
            });
        }
        else
        {
            Snackbar.Add("Invalid environment type. Unable to update.", Severity.Error);
        }
    }

    private void BackupEnvironment(object environment)
    {
        if (environment is Environment castedEnvironment)
        {
            environmentBeforeEdit = castedEnvironment with { Rules = castedEnvironment.Rules?.ToList() ?? [] };
        }
        else
        {
            throw new InvalidCastException("The provided object is not of type Environment.");
        }
    }

    private void ResetEnvironment(object environment)
    {
        if (environment is Environment castedEnvironment)
        {
            castedEnvironment.Name = environmentBeforeEdit.Name;
        }
        else
        {
            throw new InvalidCastException("The provided object is not of type Environment.");
        }
    }

    private async void AddNewEnvironment()
    {
        var parameters = new DialogParameters
        {
            { nameof(AddEnvironmentDialog.ContentText), "Enter the name of the new environment you want to create." },
            { nameof(AddEnvironmentDialog.ButtonText), "Create" },
            { nameof(AddEnvironmentDialog.Color), Color.Success }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

        var dialog = await DialogService.ShowAsync<AddEnvironmentDialog>("Add Environment", parameters, options);

        var result = await dialog.Result;

        if (!result.Canceled && result.Data is string environmentName)
        {
            try
            {
                var environment = await EnvironmentsApiClient.CreateEnvironmentAsync(environmentName);
                Snackbar.Add($"Environment '{environment.Name}' created successfully.", Severity.Success);
                // Refresh the environment list
                await RefreshEnvironmentList();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error creating environment: {ex.Message}", Severity.Error);
            }
        }
        else
        {
            Snackbar.Add("Operation cancelled.", Severity.Info);
        }
    }

    private async Task RefreshEnvironmentList()
    {
        isLoading = true; // Start loading
        try
        {
            environments = (await EnvironmentsApiClient.GetAllEnvironmentsAsync())?.ToList() ?? [];
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

    // Filter function for environments
    private bool FilterFunc(Environment env)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;

        return env.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase);
    }
}
