using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Server.Models;

namespace Yugen.HomeBudget.Server.Components.Layout
{
    public partial class TopMenu
    {
        private static bool IsAccount => Routes.CurrentUrl?.StartsWith("Account") ?? false;

        [Parameter]
        public ForwardRef TargetForwardRef { get; set; } = new ForwardRef();
    }
}