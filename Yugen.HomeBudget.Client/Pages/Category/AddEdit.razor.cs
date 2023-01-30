using Microsoft.AspNetCore.Components;

namespace Yugen.HomeBudget.Client.Pages.Category
{
    public partial class AddEdit
    {
        [Parameter]
        public int? Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //ViewModel.PropertyChanged += (_, _) => StateHasChanged();

            await ViewModel.OnInitializedAsync(Id);

            await base.OnInitializedAsync();
        }
    }
}