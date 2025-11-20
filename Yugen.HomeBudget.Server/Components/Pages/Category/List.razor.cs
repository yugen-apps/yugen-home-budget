namespace Yugen.HomeBudget.Server.Components.Pages.Category
{
    public partial class List
    {
        protected override async Task OnInitializedAsync()
        {
            await ViewModel.LoadDataAsync();

            await base.OnInitializedAsync();
        }
    }
}