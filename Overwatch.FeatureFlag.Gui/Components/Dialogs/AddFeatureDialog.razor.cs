using MudBlazor;

namespace Overwatch.FeatureFlag.Gui.Components.Dialogs;

public partial class AddFeatureDialog : ComponentBase
{
    [Inject] private FeaturesApiClient FeaturesApiClient { get; set; } = default!;
    [Inject] private EnvironmentsApiClient EnvironmentsApiClient { get; set; } = default!;
    
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    [Parameter] public string ContentText { get; set; } = "";
    [Parameter] public string ButtonText { get; set; } = "";
    [Parameter] public Color Color { get; set; }
    
    private string FeatureName { get; set; } = string.Empty;
    private string Value { get; set; } = "Nothing selected";
    private IEnumerable<string> Options { get; set; } = new HashSet<string>();
    private List<Environment> Environments { get; set; } = [];
    protected override async Task OnInitializedAsync()
    {
        Environments = (await EnvironmentsApiClient.GetAllEnvironmentsAsync()).ToList();
    }

    private async Task Submit()
    {
        await FeaturesApiClient.CreateFeatureAsync(FeatureName, Environments.Where(x=>Options.Contains(x.Name)).Select(e => e.Id).ToArray());
        Snackbar.Add($"Feature '{FeatureName}' added successfully.", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}