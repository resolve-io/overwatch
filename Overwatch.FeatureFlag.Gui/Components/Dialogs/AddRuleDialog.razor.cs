using MudBlazor;
using Overwatch.FeatureFlag.Interface;

namespace Overwatch.FeatureFlag.Gui.Components.Dialogs;

public partial class AddRuleDialog : ComponentBase
{
    [Inject] private FeaturesApiClient FeaturesApiClient { get; set; } = default!;
    [Inject] private EnvironmentsApiClient EnvironmentsApiClient { get; set; } = default!;
    [Inject] private RulesApiClient RulesApiClient { get; set; } = default!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    [Parameter] public string ContentText { get; set; } = "";
    [Parameter] public string ButtonText { get; set; } = "";
    [Parameter] public Color Color { get; set; }

    private List<Feature> Features = new();
    private List<Environment> Environments = new();

    private string Tenant { get; set; } = "*";
    private Guid? FeatureId { get; set; }
    private Guid? EnvironmentId { get; set; }
    public bool IsEnabled { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        Features = (await FeaturesApiClient.GetAllFeaturesAsync()).ToList();
        Environments = (await EnvironmentsApiClient.GetAllEnvironmentsAsync()).ToList();
    }

    private async Task Submit()
    {
        await RulesApiClient.CreateRuleAsync(new CreateRuleRequest(FeatureId.Value, EnvironmentId, Tenant, IsEnabled));
        Snackbar.Add($"Rule added successfully.", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
        // Logic to close the dialog after submission
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}