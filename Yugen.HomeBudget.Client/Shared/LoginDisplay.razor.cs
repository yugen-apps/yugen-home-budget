using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Client.Services;

namespace Yugen.HomeBudget.Client.Shared
{
    public partial class LoginDisplay
    {
        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        [Inject]
        private CustomStateProvider? AuthStateProvider { get; set; }

        private async Task BeginLogOut()
        {
            if (AuthStateProvider != null)
            {
                await AuthStateProvider.Logout();
            }
            NavigationManager?.NavigateTo("/");
        }
    }
}