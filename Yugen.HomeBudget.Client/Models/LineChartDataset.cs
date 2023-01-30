namespace Yugen.HomeBudget.Client.Models;

public class LineChartDataset
{
    public LineChartDataset(
        string label,
        string borderColor,
        int[] data)
    {
        Label = label;
        BorderColor = borderColor;
        Data = data;
    }

    public string Label { get; set; }

    public string BackgroundColor { get; set; } = "transparent";

    public string BorderColor { get; set; }

    public int[] Data { get; set; }

    public string PointBackgroundColor { get; set; } = "transparent";

    public string PointHoverBackgroundColor { get; set; } = "#4a6cf7";

    public string PointBorderColor { get; set; } = "transparent";

    public string PointHoverBorderColor { get; set; } = "#fff";

    public int PointHoverBorderWidth { get; set; } = 3;

    public int PointBorderWidth { get; set; } = 5;

    public int PointRadius { get; set; } = 5;

    public int PointHoverRadius { get; set; } = 5;
}