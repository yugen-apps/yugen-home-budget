using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Shared.Contants;

namespace Yugen.HomeBudget.Client.Shared;

public class RedirectToLogin : ComponentBase
{
    private readonly NavigationManager _navigationManager;

    public RedirectToLogin(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    protected override void OnInitialized()
    {
        _navigationManager.NavigateTo(PageConstants.LoginUrl);
    }
}