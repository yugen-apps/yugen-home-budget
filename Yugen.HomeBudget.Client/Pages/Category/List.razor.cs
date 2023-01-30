namespace Yugen.HomeBudget.Client.Pages.Category
{
    public partial class List
    {
        protected override async Task OnInitializedAsync()
        {
            //ViewModel.PropertyChanged += (_, _) => StateHasChanged();

            await ViewModel.LoadDataAsync();

            await base.OnInitializedAsync();
        }
    }
}