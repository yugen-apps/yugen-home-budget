namespace Yugen.HomeBudget.Client.Models;

public class BarChartDataset
{
    public BarChartDataset(
        string label,
        string backgroundColor,
        int[] data)
    {
        Label = label;
        BackgroundColor = backgroundColor;
        Data = data;
    }

    public string Label { get; set; }

    public string BackgroundColor { get; set; }

    public string BarThickness { get; set; } = "flex";

    public int MaxBarThickness { get; set; } = 8;

    public int[] Data { get; set; }
}