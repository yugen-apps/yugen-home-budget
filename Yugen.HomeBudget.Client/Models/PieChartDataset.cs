namespace Yugen.HomeBudget.Client.Models;

public class PieChartDataset
{
    public PieChartDataset(
        string[] backgroundColor,
        int[] data)
    {
        BackgroundColor = backgroundColor;
        Data = data;
    }

    public string[] BackgroundColor { get; set; }

    public int[] Data { get; set; }
}