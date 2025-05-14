namespace Yugen.HomeBudget.Client.Components.Pages
{
    public partial class Home
    {
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ViewModel.LoadPieChartData();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            //ViewModel.PropertyChanged += (_, _) => StateHasChanged();

            await ViewModel.LoadDataAsync();

            await base.OnInitializedAsync();
        }
    }
}