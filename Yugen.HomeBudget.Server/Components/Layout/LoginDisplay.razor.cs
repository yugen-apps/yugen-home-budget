using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Yugen.HomeBudget.Server.Services;

namespace Yugen.HomeBudget.Server.Components.Layout
{
    public partial class LoginDisplay
    {
        private string UserName;
        private string Avatar = "assets/images/favicon.png";

        [Inject]
        private AuthService AuthService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            var currentUser = await AuthService.GetCurrentUserAsync();
            UserName = currentUser?.UserName;
            if (currentUser?.Avatar != null)
            {
                Avatar = currentUser?.Avatar;
            }
        }
    }
}