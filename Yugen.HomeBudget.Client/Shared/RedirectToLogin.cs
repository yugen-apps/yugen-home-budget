using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Helpers;

namespace Yugen.HomeBudget.Client.Shared;

public class RedirectToLogin : ComponentBase
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    protected override void OnInitialized()
    {
        _navigationManager.NavigateTo(PageHelper.AuthHref(PageConstants.Login));
    }

    //[CascadingParameter]
    //Task<AuthenticationState> AuthenticationState { get; set; }

    //protected override async void OnInitialized()
    //{
    //    if (!(await AuthenticationState).User.Identity.IsAuthenticated)
    //    {
    //        _navigationManager.NavigateTo("authentication/login");
    //    }
    //}
}