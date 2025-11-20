using Blazorise;

namespace Yugen.HomeBudget.Server.Models;

public class TotalExpense
{
    public TotalExpense(
        decimal current,
        decimal compareTo)
    {
        Current = current;
        CompareTo = compareTo;
        Percentage = Percent(current, compareTo);

        Icon = Percentage switch
        {
            > 0 => IconName.ArrowUp,
            < 0 => IconName.ArrowDown,
            _ => IconName.ArrowRight
        };
    }

    public TotalExpense(decimal current)
    {
        Current = current;
        CompareTo = 0;
        Percentage = 0;

        Icon = Percentage switch
        {
            > 0 => IconName.ArrowUp,
            < 0 => IconName.ArrowDown,
            _ => IconName.ArrowRight
        };
    }

    public decimal CompareTo { get; set; }

    public decimal Current { get; set; }

    public IconName Icon { get; set; }

    public decimal Percentage { get; set; }

    private static decimal Percent(decimal current, decimal compareTo)
    {
        if (current == 0)
        {
            current = 1;
        }
        if (compareTo == 0)
        {
            compareTo = 1;
        }
        return decimal.Round((current - compareTo) / Math.Abs(compareTo) * 100, 2);
    }
}