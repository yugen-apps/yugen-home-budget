using Blazorise.Localization;
using Microsoft.AspNetCore.Components;

namespace Yugen.HomeBudget.Server.Components.Layout
{
    public partial class LanguageSelector
    {
        [Inject] protected ITextLocalizerService? LocalizationService { get; set; }

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
