namespace Yugen.HomeBudget.Client.Models;

public class PieChartDataset
{
    public PieChartDataset(
        List<string> backgroundColor,
        List<int> data)
    {
        BackgroundColor = backgroundColor;
        Data = data;
    }

    public List<string> BackgroundColor { get; set; } = [];

    public List<int> Data { get; set; } = [];
}