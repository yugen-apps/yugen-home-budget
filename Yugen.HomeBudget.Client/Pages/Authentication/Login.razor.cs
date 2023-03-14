using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Client.Services;
using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Client.Pages.Authentication
{
    public partial class Login
    {
        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private CustomStateProvider _authStateProvider { get; set; }

        private LoginRequest loginRequest { get; set; } = new LoginRequest();

        private string error { get; set; }

        private async Task OnSubmit()
        {
            error = null;
            try
            {
                await _authStateProvider.Login(loginRequest);
                _navigationManager.NavigateTo("");
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }
    }
}