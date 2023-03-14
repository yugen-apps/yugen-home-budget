using Microsoft.AspNetCore.Components;

namespace Yugen.HomeBudget.Client.Components
{
    public partial class TitleComponent
    {
        [Parameter]
        public string PageTitle { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Breadcrumb { get; set; }
    }
}