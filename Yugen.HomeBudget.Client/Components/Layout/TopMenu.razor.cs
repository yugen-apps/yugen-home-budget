using Blazorise.Localization;
using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Client.Models;
using Yugen.HomeBudget.Client.Services;

namespace Yugen.HomeBudget.Client.Components.Layout
{
    public partial class TopMenu
    {
        private string? _identityName;

        private string? _image = "assets/images/logo.png";

        [CascadingParameter] protected Theme? Theme { get; set; }

        [Parameter] public ForwardRef TargetForwardRef { get; set; }

        [Parameter] public EventCallback<string> ThemeColorChanged { get; set; }

        [Parameter] public EventCallback<bool> ThemeEnabledChanged { get; set; }

        [Parameter] public EventCallback<bool> ThemeGradientChanged { get; set; }

        [Parameter] public EventCallback<bool> ThemeRoundedChanged { get; set; }

        [Inject] protected ITextLocalizerService? LocalizationService { get; set; }

        [Inject] private CustomStateProvider? AuthStateProvider { get; set; }

        [Inject] private NavigationManager? NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await SelectCulture("en-US");

            if (AuthStateProvider != null)
            {
                var authenticationState = await AuthStateProvider.GetAuthenticationStateAsync();
                _identityName = authenticationState?.User.Identity?.Name;

                //CurrentUser? currentUser = await AuthStateProvider.GetCurrentUser();
                //_identityName = currentUser?.UserName;
            }

            await base.OnInitializedAsync();
        }

        private Task SelectCulture(string name)
        {
            LocalizationService!.ChangeLanguage(name);

            return Task.CompletedTask;
        }

        private async Task BeginLogOut()
        {
            if (AuthStateProvider != null)
            {
                await AuthStateProvider.Logout();
            }
            NavigationManager?.NavigateTo("/");
        }

        private async Task NavigateTo(string url)
        {
            NavigationManager?.NavigateTo(url);
        }
    }
}