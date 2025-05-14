namespace Yugen.HomeBudget.Client.Components.Pages.Expense
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