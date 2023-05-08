using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Yugen.HomeBudget.Client.Models;

namespace Yugen.HomeBudget.Client.Components
{
    public partial class BarChartComponent
    {
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/BarChartComponent.razor.js");

            var labels = new[] { "Jan", "Fab", "Mar", "Apr", "May", "Jun" };
            BarChartDataset[] barChartDataset =
            {
                new BarChartDataset("", "#4A6CF7", new []{600, 700, 1000, 700, 650, 800}),
                new BarChartDataset("", "#d50100", new []{690, 740, 720, 1120, 876, 900})
            };

            await module.InvokeVoidAsync("newBarChart", "Chart4", labels, barChartDataset);
        }
    }
}