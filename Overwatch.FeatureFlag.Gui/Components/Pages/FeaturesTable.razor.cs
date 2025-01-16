using Microsoft.Extensions.Hosting;
using MudBlazor;
using Overwatch.FeatureFlag.Gui.Components.Dialogs;
using Overwatch.FeatureFlag.Interface;

namespace Overwatch.FeatureFlag.Gui.Components.Pages;

public partial class FeaturesTable : ComponentBase
{
    private string searchString = "";
    private Feature? selectedFeature;
    private List<Feature> features = new();
    private List<Feature> filteredFeatures => features.Where(FilterFunc).ToList();
    private Feature featureBeforeEdit;
    private bool isLoading = true;

    [Inject] private FeaturesApiClient FeaturesApiClient { get; set; } = default!;
    
    protected override async Task OnInitializedAsync()
    {
        await RefreshEnvironmentList();
    }

    private void CommitFeature(object feature)
    {
        if (feature is Feature castedFeature)
        {
            //Task.Run(async () =>
            //{
            //    try
            //    {
            //        await FeaturesApiClient.CreateRuleAsync(new CreateRuleRequest(selectedFeature.Id, sel, castedFeature.Name, castedFeature.IsEnabled));
            //        Snackbar.Add($"Feature '{castedFeature.Name}' updated successfully.", Severity.Success);
            //    }
            //    catch (Exception ex)
            //    {
            //        Snackbar.Add($"Error updating feature '{castedFeature.Name}': {ex.Message}", Severity.Error);
            //    }
            //});
        }
        else
        {
            Snackbar.Add("Invalid feature type. Unable to update.", Severity.Error);
        }
    }

    private void BackupFeature(object feature)
    {
        if (feature is Feature castedFeature)
        {
            featureBeforeEdit = new Feature
            {
                Name = castedFeature.Name,
                IsEnabled = castedFeature.IsEnabled,
                DateCreated = castedFeature.DateCreated,
                DateModified = castedFeature.DateModified
            };
        }
        else
        {
            throw new InvalidCastException("The provided object is not of type Feature.");
        }
    }

    private void ResetFeature(object feature)
    {
        if (feature is Feature castedFeature)
        {
            castedFeature.Name = featureBeforeEdit.Name;
            castedFeature.IsEnabled = featureBeforeEdit.IsEnabled;
        }
        else
        {
            throw new InvalidCastException("The provided object is not of type Feature.");
        }
    }

    private async Task AddNewFeature()
    {
        var parameters = new DialogParameters
        {
            { "ContentText", "Enter the feature details below." },
            { "ButtonText", "Create Feature" },
            { "Color", Color.Primary }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

        var dialog = await DialogService.ShowAsync<AddFeatureDialog>("Create Feature", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data is string featureName)
        {
            await RefreshEnvironmentList();
        }
    }

    private bool FilterFunc(Feature feature)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        return feature.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase);
    }

    private async Task RefreshEnvironmentList()
    {
        isLoading = true; // Start loading
        try
        {
            features = (await FeaturesApiClient.GetAllFeaturesAsync()).ToList() ?? [];
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

