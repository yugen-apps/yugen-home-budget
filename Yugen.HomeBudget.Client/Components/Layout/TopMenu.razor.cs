using Blazorise.Localization;
using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Client.Models;

namespace Yugen.HomeBudget.Client.Components.Layout
{
    public partial class TopMenu
    {
        [Parameter]
        public ForwardRef TargetForwardRef { get; set; }

        [Parameter] public EventCallback<string> ThemeColorChanged { get; set; }

        [Parameter] public EventCallback<bool> ThemeEnabledChanged { get; set; }

        [Parameter] public EventCallback<bool> ThemeGradientChanged { get; set; }

        [Parameter] public EventCallback<bool> ThemeRoundedChanged { get; set; }

        [Inject] protected ITextLocalizerService? LocalizationService { get; set; }

        [CascadingParameter] protected Theme? Theme { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await SelectCulture("en-US");

            await base.OnInitializedAsync();
        }

        private Task SelectCulture(string name)
        {
            LocalizationService!.ChangeLanguage(name);

            return Task.CompletedTask;
        }
    }
}