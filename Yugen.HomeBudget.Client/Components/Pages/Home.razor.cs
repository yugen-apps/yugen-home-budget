using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Client.Components.Pages
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