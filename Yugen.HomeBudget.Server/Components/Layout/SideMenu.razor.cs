using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Yugen.HomeBudget.Server.Models;
using Yugen.HomeBudget.Server.Navigation;

namespace Yugen.HomeBudget.Server.Components.Layout
{
    public partial class SideMenu
    {
        [Parameter]
        public ForwardRef TargetForwardRef { get; set; } = new ForwardRef();

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        private string logoImg = "<img src = \"assets/images/logo.png\" style=\"width:32px; height: 32px\" />";

        private bool _pagesBarVisible = true;

        private RenderFragment customIcon => (builder) => builder.AddMarkupContent(0, $"{logoImg}");

        private List<MenuItem> MenuItems => MenuConstants.MenuItems;

        private string? currentUrl;

        protected override void OnInitialized()
        {
            currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
            StateHasChanged();
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}