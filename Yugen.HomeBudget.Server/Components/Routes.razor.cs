using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Yugen.HomeBudget.Server.Components;

public partial class Routes
{
    public static string CurrentUrl { get; private set; }

    public static bool IsAccount => CurrentUrl?.StartsWith("Account") ?? false;

    public static bool IsAccountManage => CurrentUrl?.StartsWith("Account/Manage") ?? false;

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        if (NavigationManager == null)
        {
            return;
        }

        CurrentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        CurrentUrl = NavigationManager?.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }

    public void Dispose()
    {
        if (NavigationManager == null)
        {
            return;
        }

        NavigationManager.LocationChanged -= OnLocationChanged;
    }
}
