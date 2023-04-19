using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Yugen.HomeBudget.Client.Models;

namespace Yugen.HomeBudget.Client.Components
{
    public partial class PieChartComponent
    {
        [Parameter]
        public PieChartData? PieChartData { get; set; }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        private IJSObjectReference module;

        protected override async Task OnInitializedAsync()
        {
            module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/PieChartComponent.razor.js");
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);

            if (PieChartData != null)
            {
                await module.InvokeVoidAsync("newPieChart", "pieChart", PieChartData.Labels, PieChartData.PieChartDataset);
            }
        }
    }
}