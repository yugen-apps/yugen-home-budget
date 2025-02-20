using Microsoft.AspNetCore.Components;

namespace Yugen.HomeBudget.Client.Components.Layout
{
    public partial class SideMenu
    {
        private string logoImg = "<img src = \"assets/images/logo.png\" style=\"width:32px; height: 32px\" />";

        private RenderFragment customIcon => (builder) => builder.AddMarkupContent(0, $"{logoImg}");
    }
}