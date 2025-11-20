using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Yugen.HomeBudget.Server.Components
{
    public partial class Routes
    {
        private readonly Theme theme = new()
        {
            BarOptions = new()
            {
                HorizontalHeight = "72px"
            },
            ColorOptions = new()
            {
                Primary = "#0288D1",
                Secondary = "#A65529",
                Success = "#23C02E",
                Info = "#9BD8FE",
                Warning = "#F8B86C",
                Danger = "#F95741",
                Light = "#F0F0F0",
                Dark = "#535353",
            },
            BackgroundOptions = new()
            {
                Primary = "#0288D1",
                Secondary = "#A65529",
                Success = "#23C02E",
                Info = "#9BD8FE",
                Warning = "#F8B86C",
                Danger = "#F95741",
                Light = "#F0F0F0",
                Dark = "#535353",
            },
            InputOptions = new()
            {
                CheckColor = "#0288D1",
            }
        };

        public static string? CurrentUrl { get; private set; }

        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            if (NavigationManager == null)
            {
                return;
            }

            CurrentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
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
}
