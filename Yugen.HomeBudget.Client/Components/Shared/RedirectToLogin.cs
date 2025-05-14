using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Shared.Contants;

namespace Yugen.HomeBudget.Client.Components.Shared;

public class RedirectToLogin : ComponentBase
{
    [Inject]
    private NavigationManager? NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        NavigationManager?.NavigateTo(PageConstants.LoginUrl);
    }
}