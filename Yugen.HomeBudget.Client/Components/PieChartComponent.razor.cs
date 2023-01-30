using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Yugen.HomeBudget.Client.Models;

namespace Yugen.HomeBudget.Client.Components
{
    public partial class PieChartComponent
    {
        private IJSObjectReference? _module;

        [Parameter]
        public PieChartData? PieChartData { get; set; }

        [Inject]
        private IJSRuntime? JsRuntime { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);

            if (PieChartData != null &&
                _module != null)
            {
                await _module.InvokeVoidAsync("newPieChart", "pieChart", PieChartData.Labels,
                    PieChartData.PieChartDataset);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (JsRuntime != null)
            {
                _module = await JsRuntime.InvokeAsync<IJSObjectReference>("import",
                    "./Components/PieChartComponent.razor.js");
            }
        }

        // https://learn.microsoft.com/en-us/aspnet/core/blazor/performance?view=aspnetcore-7.0
    }
}