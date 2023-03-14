using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
}