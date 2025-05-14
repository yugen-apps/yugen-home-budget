using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Client.Components.Pages.Expense
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