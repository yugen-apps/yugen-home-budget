namespace Yugen.HomeBudget.Client.Components.Pages.Info
{
    public partial class Index
    {
        protected override async Task OnInitializedAsync()
        {
            await ViewModel.LoadDataAsync();

            await base.OnInitializedAsync();
        }
    }
}
