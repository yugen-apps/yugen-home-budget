using Microsoft.AspNetCore.Components;

namespace Yugen.HomeBudget.Client.Components.Layout
{
    public partial class ThemeColorSelector
    {
        [Parameter]
        public string? Value { get; set; }

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        private Task OnThemeColorSelect(string value)
        {
            Value = value;
            return ValueChanged.InvokeAsync(value);
        }
    }
}