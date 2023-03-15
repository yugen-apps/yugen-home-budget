using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Shared.Contants;

namespace Yugen.HomeBudget.Client.Shared;

public class RedirectToLogin : ComponentBase
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    protected override void OnInitialized()
    {
        _navigationManager.NavigateTo(PageConstants.LoginUrl);
    }
}