using Blazorise.DataGrid;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Server.Components.Pages.Expense
{
    public partial class List
    {
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