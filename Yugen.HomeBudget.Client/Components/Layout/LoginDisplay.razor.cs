using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Client.Services;

namespace Yugen.HomeBudget.Client.Components.Layout
{
    public partial class LoginDisplay
    {
        private string? _identityName;

        private string? _image = "assets/images/logo.png";

        [Inject]
        private CustomStateProvider? AuthStateProvider { get; set; }

        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (AuthStateProvider != null)
            {
                var authenticationState = await AuthStateProvider.GetAuthenticationStateAsync();
                _identityName = authenticationState?.User.Identity?.Name;

                //CurrentUser? currentUser = await AuthStateProvider.GetCurrentUser();
                //_identityName = currentUser?.UserName;
            }
        }

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