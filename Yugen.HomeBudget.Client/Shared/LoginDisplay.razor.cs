using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Client.Services;

namespace Yugen.HomeBudget.Client.Shared
{
    public partial class LoginDisplay
    {
        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private CustomStateProvider _authStateProvider { get; set; }

        private async Task BeginLogOut()
        {
            await _authStateProvider.Logout();
            _navigationManager.NavigateTo("/");
        }
    }
}