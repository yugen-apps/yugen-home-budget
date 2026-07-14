using Microsoft.AspNetCore.Components;
using Yugen.Home.Budget.Server.Models.Home;

namespace Yugen.Home.Budget.Server.Components.Shared;

public partial class HomeCardComponent
{
    [Parameter]
    public string Header { get; set; }

    [Parameter]
    public string IconName { get; set; }

    [Parameter]
    public TotalExpense TotalExpense { get; set; }

    [Parameter]
    public TotalAccrued TotalAccrued { get; set; }

    public decimal? Current { get; private set; }

    public decimal? CompareTo { get; private set; }

    public string Icon { get; private set; }

    public decimal? Percentage { get; private set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (TotalAccrued != null)
        {
            Current = TotalAccrued.Current;
            CompareTo = TotalAccrued.CompareTo;
            Icon = TotalAccrued.Icon;
            Percentage = TotalAccrued.Percentage;
        }
        else if (TotalExpense != null)
        {
            Current = TotalExpense.Current;
            CompareTo = TotalExpense.CompareTo;
            Icon = TotalExpense.Icon;
            Percentage = TotalExpense.Percentage;
        }
    }
}