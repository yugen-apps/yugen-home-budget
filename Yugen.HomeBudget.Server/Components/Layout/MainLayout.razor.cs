using MudBlazor;

namespace Yugen.HomeBudget.Server.Components.Layout
{
    public partial class MainLayout
    {
        private bool _drawerOpen = true;

        private void DrawerToggle() => _drawerOpen = !_drawerOpen;

        private bool _isDarkMode;

        private void ToggleTheme() => _isDarkMode = !_isDarkMode;

        private string ThemeIcon => _isDarkMode ? Icons.Material.Filled.LightMode : Icons.Material.Filled.DarkMode;

        private string ThemeLabel => _isDarkMode ? "Light mode" : "Dark mode";
    }
}