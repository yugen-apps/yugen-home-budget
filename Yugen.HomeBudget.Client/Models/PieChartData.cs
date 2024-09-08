namespace Yugen.HomeBudget.Client.Models;

public class PieChartData
{
    public PieChartData(
        List<string> labels,
        List<PieChartDataset> pieChartDataset)
    {
        Labels = labels;
        PieChartDataset = pieChartDataset;
    }

    public List<string> Labels { get; set; } = [];

    public List<PieChartDataset> PieChartDataset { get; set; } = [];
}