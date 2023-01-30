namespace Yugen.HomeBudget.Client.Shared
{
    public partial class MainLayout
    {
        private bool _activeNavMenu = false;

        private string? NavMenuCssClass => _activeNavMenu ? "active" : null;

        private void ToggleNavMenu()
        {
            _activeNavMenu = !_activeNavMenu;
        }
    }
}