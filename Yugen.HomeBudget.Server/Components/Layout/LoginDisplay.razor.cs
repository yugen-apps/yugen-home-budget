using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;
using Yugen.HomeBudget.Server.Services;

namespace Yugen.HomeBudget.Server.Components.Layout
{
    public partial class LoginDisplay
    {
        [Inject]
        private AuthService AuthService { get; set; } = default!;
        [Parameter]
        public EventCallback<bool> DarkModeChanged { get; set; }

        private static bool IsAccount => Routes.CurrentUrl?.StartsWith("Account") ?? false;

        private bool _isDarkMode;
        private string Avatar;
        private string GivenName;
        private string UserName;

        protected override async Task OnInitializedAsync()
        {
            var currentUser = await AuthService.GetCurrentUserAsync();
            
            Avatar = currentUser?.Avatar ?? "assets/images/favicon.png";
            GivenName = currentUser?.GivenName ?? string.Empty;
            UserName = currentUser?.UserName ?? string.Empty;
        }

        public void ToggleTheme()
        {
            _isDarkMode = !_isDarkMode;
            DarkModeChanged.InvokeAsync(_isDarkMode);
        }

        public string ThemeIcon => _isDarkMode ? Icons.Material.Filled.LightMode : Icons.Material.Filled.DarkMode;

        public string ThemeLabel => _isDarkMode ? "Light mode" : "Dark mode";
    }
}