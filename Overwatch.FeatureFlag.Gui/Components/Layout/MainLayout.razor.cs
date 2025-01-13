using MudBlazor;

namespace Overwatch.FeatureFlag.Gui.Components.Layout;

public partial class MainLayout : LayoutComponentBase
{
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public EventCallback OnDarkModeToggle { get; set; }

    private bool drawerOpen = true;

    protected override async Task OnInitializedAsync()
    {
    }
    
    private async Task DrawerToggle()
    {
        drawerOpen = !drawerOpen; // Immediate state change for the UI.
    }
}