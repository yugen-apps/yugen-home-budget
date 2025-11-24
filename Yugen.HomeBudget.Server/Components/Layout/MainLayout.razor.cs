using System.Threading.Tasks;

namespace Yugen.HomeBudget.Server.Components.Layout
{
    public partial class MainLayout
    {
        private bool _drawerOpen = true;
        private bool _isDarkMode;

        private void DrawerToggle() => _drawerOpen = !_drawerOpen;

        private Task OnDarkModeChanged(bool value)
        {           
            _isDarkMode = value;
            return Task.CompletedTask;
        }
    }
}