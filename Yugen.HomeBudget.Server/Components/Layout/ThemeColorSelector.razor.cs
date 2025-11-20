using Blazorise;
using Microsoft.AspNetCore.Components;

namespace Yugen.HomeBudget.Server.Components.Layout
{
    public partial class ThemeColorSelector
    {
        [CascadingParameter]
        protected Theme? Theme { get; set; }

        private Task OnThemeEnabledChanged(bool value)
        {
            if (Theme is null)
            {
                return Task.CompletedTask;
            }

            Theme.Enabled = value;

            return InvokeAsync(Theme.ThemeHasChanged);
        }

        private Task OnThemeGradientChanged(bool value)
        {
            if (Theme is null)
            {
                return Task.CompletedTask;
            }

            Theme.IsGradient = value;

            return InvokeAsync(Theme.ThemeHasChanged);
        }

        private Task OnThemeRoundedChanged(bool value)
        {
            if (Theme is null)
            {
                return Task.CompletedTask;
            }

            Theme.IsRounded = value;

            return InvokeAsync(Theme.ThemeHasChanged);
        }

        private Task OnThemeColorSelect(string value)
        {
            if (Theme is null)
            {
                return Task.CompletedTask;
            }

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
    }
}