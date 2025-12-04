using MudBlazor;
using Yugen.HomeBudget.Application.Models.Expense;

namespace Yugen.HomeBudget.Server.Components.Pages.Expense.List;

public partial class ExpenseList
{
    private MudDataGrid<ResponseExpenseDto> DataGrid;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            ViewModel.RefreshServerDataFunc ??= DataGrid.ReloadServerData;
        }

        base.OnAfterRender(firstRender);
    }
}

