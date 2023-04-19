namespace Yugen.HomeBudget.Client.Models;

public class PieChartData
{
    public PieChartData(
        string[] labels,
        PieChartDataset[] pieChartDataset)
    {
        Labels = labels;
        PieChartDataset = pieChartDataset;
    }

    public string[] Labels { get; set; }

    public PieChartDataset[] PieChartDataset { get; set; }
}