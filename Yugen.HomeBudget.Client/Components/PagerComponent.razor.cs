using Microsoft.AspNetCore.Components;

namespace Yugen.HomeBudget.Client.Components
{
    public partial class PagerComponent
    {
        [Parameter]
        public int PageIndex { get; set; }

        [Parameter]
        public int TotalPages { get; set; }

        [Parameter]
        public bool HasPreviousPage { get; set; }

        [Parameter]
        public bool HasNextPage { get; set; }

        [Parameter]
        public EventCallback<int> OnClick { get; set; }

        public bool IsFirstPage => PageIndex == 1;

        public bool IsLastPage => PageIndex == TotalPages;

        public int PreviousPage => PageIndex - 1;

        public int NextPage => PageIndex + 1;
    }
}