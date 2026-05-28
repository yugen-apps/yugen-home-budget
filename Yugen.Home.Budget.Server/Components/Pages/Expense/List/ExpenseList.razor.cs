using MudBlazor;
using Yugen.Home.Budget.Application.Models.Expense;

namespace Yugen.Home.Budget.Server.Components.Pages.Expense.List;

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

