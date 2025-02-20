using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Client.Models;

namespace Yugen.HomeBudget.Client.Components.Layout
{
    public partial class SideMenu
    {
        [Parameter]
        public ForwardRef TargetForwardRef { get; set; }

        private string logoImg = "<img src = \"assets/images/logo.png\" style=\"width:32px; height: 32px\" />";

        private RenderFragment customIcon => (builder) => builder.AddMarkupContent(0, $"{logoImg}");
    }
}