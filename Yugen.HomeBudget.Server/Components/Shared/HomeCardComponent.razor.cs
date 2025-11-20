using Blazorise;
using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Server.Models;

namespace Yugen.HomeBudget.Server.Components.Shared
{
    public partial class HomeCardComponent
    {
        [Parameter]
        public string? Header { get; set; }

        [Parameter]
        public IconName? IconName { get; set; }

        [Parameter]
        public TotalExpense? TotalExpense { get; set; }

        [Parameter]
        public TotalAccrued? TotalAccrued { get; set; }

        [Parameter]
        public string? Footer { get; set; }

        public decimal? Current { get; private set; }

        public IconName? Icon { get; private set; }

        public decimal? Percentage { get; private set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (TotalAccrued != null)
            {
                Current = TotalAccrued.Current;
                Icon = TotalAccrued.Icon;
                Percentage = TotalAccrued.Percentage;
            }
            else if (TotalExpense != null)
            {
                Current = TotalExpense.Current;
                Icon = TotalExpense.Icon;
                Percentage = TotalExpense.Percentage;
            }
        }
    }
}