using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Client.Pages.Authentication
{
    public partial class Register
    {
        RegisterRequest registerRequest { get; set; } = new RegisterRequest();
        string error { get; set; }

        async Task OnSubmit()
        {
            error = null;
            try
            {
                await authStateProvider.Register(registerRequest);
                navigationManager.NavigateTo("");
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }
    }
}