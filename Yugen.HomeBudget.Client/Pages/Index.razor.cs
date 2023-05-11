namespace Yugen.HomeBudget.Client.Pages
{
    public partial class Index
    {
        protected override async Task OnInitializedAsync()
        {
            //ViewModel.PropertyChanged += (_, _) => StateHasChanged();

            await ViewModel.LoadDataAsync();

            await base.OnInitializedAsync();
        }
    }
}