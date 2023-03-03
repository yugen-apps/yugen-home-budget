using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Client.Pages.Authentication
{
    public partial class Register
    {
        private RegisterRequest registerRequest { get; set; } = new RegisterRequest();

        private string error { get; set; }

        private async Task OnSubmit()
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