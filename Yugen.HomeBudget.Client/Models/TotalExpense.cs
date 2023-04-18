namespace Yugen.HomeBudget.Client.Models;

public class TotalExpense
{
    public TotalExpense(
        decimal current, 
        decimal compareTo)
    {
        Current = current;
        CompareTo = compareTo;

        if (current == 0)
        {
            current = 1;
        }
        if (compareTo == 0)
        {
            compareTo = 1;
        }
        Percentage = ((current - compareTo) / Math.Abs(compareTo)) * 100;

        Icon = Percentage switch
        {
            > 0 => Constants.ArrowUp,
            < 0 => Constants.ArrowDown,
            _ => Constants.ArrowRight
        };
    }

    public decimal Current { get; set; }

    public decimal CompareTo { get; set; }

    public decimal Percentage { get; set; }

    public string Icon { get; set; }
}