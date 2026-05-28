using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Yugen.Home.Budget.Server.Components.Pages.Expense.Details;

public partial class ExpenseDetails
{
	[Parameter]
	public int? Id { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await ViewModel.OnInitializedAsync(Id);

		ViewModel.PropertyChanged += async (sender, e) =>
		{
			await InvokeAsync(() =>
			{
				StateHasChanged();
			});
		};

		await base.OnInitializedAsync();
	}
}