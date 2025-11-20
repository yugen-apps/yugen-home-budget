using Blazorise.DataGrid;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Server.Components.Pages
{
    public partial class Home
    {
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ViewModel.LoadPieChartData();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            //ViewModel.PropertyChanged += (_, _) => StateHasChanged();

            await ViewModel.LoadDataAsync();

            await base.OnInitializedAsync();
        }

        private async Task OnReadData(DataGridReadDataEventArgs<ResponseExpenseDto> e)
        {
            if (e.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            await ViewModel.OnReadData(e.Page, e.PageSize);
        }
    }
}