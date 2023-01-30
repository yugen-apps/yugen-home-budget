using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Yugen.HomeBudget.Client.Shared
{
    public partial class NavMenu
    {
        [Inject] 
        private NavigationManager? NavigationManager { get; set; }

        [Parameter]
        public string? NavMenuCssClass { get; set; }

        protected override void OnInitialized()
        {
            if (NavigationManager != null)
            {
                NavigationManager.LocationChanged += (s, e) => StateHasChanged();
            }
        }

        private bool IsActive(string href, NavLinkMatch navLinkMatch = NavLinkMatch.Prefix)
        {
            if (NavigationManager != null)
            {
                var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri).ToLower();
                return navLinkMatch == NavLinkMatch.All ? relativePath == href.ToLower() : relativePath.StartsWith(href.ToLower());
            }

            return false;
        }

        private string GetActive(string href, NavLinkMatch navLinkMatch = NavLinkMatch.Prefix) => IsActive(href, navLinkMatch) ? "active" : "";
    }
}