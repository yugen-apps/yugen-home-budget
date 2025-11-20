using Microsoft.AspNetCore.Components;

namespace Yugen.HomeBudget.Server.Components.Shared
{
    public partial class PagerComponent
    {
        private const string PREVIOUS = "previous";
        private const string NEXT = "next";

        [Parameter]
        public string CurrentPage { get; set; } = string.Empty;

        [Parameter]
        public int PageItems { get; set; }

        [Parameter]
        public EventCallback<int> OnClick { get; set; }

        private bool IsActive(string page)
            => CurrentPage == page;

        private bool IsPageNavigationDisabled(string navigation)
        {
            if (navigation.Equals(PREVIOUS))
            {
                return CurrentPage.Equals("1");
            }
            else if (navigation.Equals(NEXT))
            {
                return CurrentPage.Equals(PageItems.ToString());
            }
            return false;
        }

        private void Previous()
        {
            var currentPageAsInt = int.Parse(CurrentPage);
            if (currentPageAsInt > 1)
            {
                CurrentPage = (currentPageAsInt - 1).ToString();
            }
        }

        private void Next()
        {
            var currentPageAsInt = int.Parse(CurrentPage);
            if (currentPageAsInt < PageItems)
            {
                CurrentPage = (currentPageAsInt + 1).ToString();
            }
        }

        private void SetActive(string page)
            => CurrentPage = page;
    }
}