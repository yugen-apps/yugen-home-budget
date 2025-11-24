using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Yugen.HomeBudget.Application.Models.Category;

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

        public void CategoryChanged(ResponseCategoryDto category)
        {
            ViewModel.Model.CategoryChanged(category);

            StateHasChanged();
        }
    }
}