using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Client.Pages.Authentication
{
    public partial class Login
    {
        LoginRequest loginRequest { get; set; } = new LoginRequest();
        string error { get; set; }

        async Task OnSubmit()
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