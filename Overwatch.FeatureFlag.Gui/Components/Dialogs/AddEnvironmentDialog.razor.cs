using MudBlazor;

namespace Overwatch.FeatureFlag.Gui.Components.Dialogs;

public partial class AddEnvironmentDialog : ComponentBase
{
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    [Parameter] public string ContentText { get; set; }

    [Parameter] public string ButtonText { get; set; }

    [Parameter] public Color Color { get; set; }

    private string EnvironmentName { get; set; } = string.Empty;

    private void Submit()
    {
        if (!string.IsNullOrWhiteSpace(EnvironmentName))
        {
            MudDialog.Close(DialogResult.Ok(EnvironmentName));
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}