using MudBlazor;
using Overwatch.FeatureFlag.Interface;

namespace Overwatch.FeatureFlag.Gui.Components.Pages;

public partial class FeaturesTable : ComponentBase
{
    private string searchString = "";
    private TableEditTrigger editTrigger = TableEditTrigger.RowClick;
    private TableApplyButtonPosition applyButtonPosition = TableApplyButtonPosition.End;
    private TableEditButtonPosition editButtonPosition = TableEditButtonPosition.End;

    private Feature? selectedFeature;
    private List<Feature> features = new();
    private List<Feature> filteredFeatures => features.Where(FilterFunc).ToList();
    private Feature featureBeforeEdit;

    [Inject] private FeaturesApiClient FeaturesApiClient { get; set; } = default!;
    
    protected override async Task OnInitializedAsync()
    {
        features = (await FeaturesApiClient.GetAllFeaturesAsync()).ToList();
    }

    private void CommitFeature(object feature)
    {
        if (feature is Feature castedFeature)
        {
            Task.Run(async () =>
            {
                try
                {
                    await FeaturesApiClient.CreateRuleAsync(new CreateRuleRequest("global", "*", castedFeature.Name, castedFeature.IsEnabled));
                    Snackbar.Add($"Feature '{castedFeature.Name}' updated successfully.", Severity.Success);
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error updating feature '{castedFeature.Name}': {ex.Message}", Severity.Error);
                }
            });
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

    private void AddNewFeature()
    {
        Snackbar.Add("Add new feature clicked.", Severity.Info);
    }

    private bool FilterFunc(Feature feature)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        return feature.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase);
    }
}

