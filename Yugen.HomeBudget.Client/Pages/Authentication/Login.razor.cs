using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Client.Pages.Authentication
{
    public partial class Login
    {
        private LoginRequest loginRequest { get; set; } = new LoginRequest();

        private string error { get; set; }

        private async Task OnSubmit()
        {
            error = null;
            try
            {
                await authStateProvider.Login(loginRequest);
                navigationManager.NavigateTo("");
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }
    }
}