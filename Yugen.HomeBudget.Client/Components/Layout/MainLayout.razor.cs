using Blazorise.Localization;

using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Client.Models;

namespace Yugen.HomeBudget.Client.Components.Layout
{
    public partial class MainLayout
    {
        ForwardRef barForwardRef = new ForwardRef();

        [Inject]
        protected ITextLocalizerService? LocalizationService { get; set; }

        [CascadingParameter]
        protected Theme? Theme { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await SelectCulture("en-US");

            await base.OnInitializedAsync();
        }

        private Task OnThemeColorChanged(string value)
        {
            if (Theme is null)
                return Task.CompletedTask;

            Theme.ColorOptions ??= new();

            Theme.BackgroundOptions ??= new();

            Theme.TextColorOptions ??= new();

            Theme.ColorOptions.Primary = value;
            Theme.BackgroundOptions.Primary = value;
            Theme.TextColorOptions.Primary = value;

            Theme.InputOptions ??= new();

            Theme.InputOptions.CheckColor = value;
            Theme.InputOptions.SliderColor = value;

            Theme.SpinKitOptions ??= new();

            Theme.SpinKitOptions.Color = value;

            return InvokeAsync(Theme.ThemeHasChanged);
        }

        private Task OnThemeEnabledChanged(bool value)
        {
            if (Theme is null)
                return Task.CompletedTask;

            Theme.Enabled = value;

            return InvokeAsync(Theme.ThemeHasChanged);
        }

        private Task OnThemeGradientChanged(bool value)
        {
            if (Theme is null)
                return Task.CompletedTask;

            Theme.IsGradient = value;

            return InvokeAsync(Theme.ThemeHasChanged);
        }

        private Task OnThemeRoundedChanged(bool value)
        {
            if (Theme is null)
                return Task.CompletedTask;

            Theme.IsRounded = value;

            return InvokeAsync(Theme.ThemeHasChanged);
        }

        private Task SelectCulture(string name)
        {
            LocalizationService!.ChangeLanguage(name);

            return Task.CompletedTask;
        }
    }
}