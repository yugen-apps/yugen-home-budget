using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Yugen.HomeBudget.Server.Components.Pages.Expense
{
    public partial class ExpenseDetails
    {
        [Parameter]
        public int? Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ViewModel.OnInitializedAsync(Id);

            StateHasChanged();

            await base.OnInitializedAsync();
        }
    }
}