using Microsoft.AspNetCore.Components;

namespace Yugen.HomeBudget.Server.Components.Pages.Expense
{
    public partial class AddEdit
    {
        [Parameter]
        public int? Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ViewModel.OnInitializedAsync(Id);

            await base.OnInitializedAsync();
        }
    }
}