using Microsoft.AspNetCore.Components;

namespace Yugen.HomeBudget.Server.Components.Shared
{
    public partial class HeaderComponent
    {
        [Parameter]
        public string? PageTitle { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Breadcrumb { get; set; }
    }
}