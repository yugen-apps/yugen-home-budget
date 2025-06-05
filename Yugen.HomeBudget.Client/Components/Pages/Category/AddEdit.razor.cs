using Microsoft.AspNetCore.Components;

namespace Yugen.HomeBudget.Client.Components.Pages.Category
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