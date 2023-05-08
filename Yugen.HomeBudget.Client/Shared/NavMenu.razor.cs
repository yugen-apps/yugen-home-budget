using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Yugen.HomeBudget.Client.Shared
{
    public partial class NavMenu
    {
        [Parameter]
        public string? NavMenuCssClass { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        protected override void OnInitialized() => _navigationManager.LocationChanged += (s, e) => StateHasChanged();

        private bool IsActive(string href, NavLinkMatch navLinkMatch = NavLinkMatch.Prefix)
        {
            var relativePath = _navigationManager.ToBaseRelativePath(_navigationManager.Uri).ToLower();
            return navLinkMatch == NavLinkMatch.All ? relativePath == href.ToLower() : relativePath.StartsWith(href.ToLower());
        }

        private string GetActive(string href, NavLinkMatch navLinkMatch = NavLinkMatch.Prefix) => IsActive(href, navLinkMatch) ? "active" : "";
    }
}