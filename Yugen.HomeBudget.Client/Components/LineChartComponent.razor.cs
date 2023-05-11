using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Yugen.HomeBudget.Client.Models;

namespace Yugen.HomeBudget.Client.Components
{
    public partial class LineChartComponent
    {
        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var module = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/LineChartComponent.razor.js");

            string[] labels = { "Jan", "Fab", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            LineChartDataset[] lineChartDatasets =
            {
                    new LineChartDataset("Revenue", "#4a6cf7", new []{80, 120, 110, 100, 130, 150, 115, 145, 140, 130, 160, 210}),
                    new LineChartDataset("Profit", "#9b51e0", new []{120, 160, 150, 140, 165, 210, 135, 155, 170, 140, 130, 200}),
                    new LineChartDataset("Order", "#f2994a", new []{180, 110, 140, 135, 100, 90, 145, 115, 100, 110, 115, 150})
                };

            await module.InvokeVoidAsync("newLineChart", "Chart3", labels, lineChartDatasets);
        }
    }
}