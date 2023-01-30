using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Yugen.HomeBudget.Client.Services;

namespace Yugen.HomeBudget.Client.Shared
{
    public partial class LoginDisplay
    {
        [Inject]
        private NavigationManager _navigation { get; set; }

        [Inject]
        private CustomStateProvider _authStateProvider { get; set; }

        async Task BeginLogOut()
        {
            await _authStateProvider.Logout();
            _navigation.NavigateTo("/");
        }
    }
}