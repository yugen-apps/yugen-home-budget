using System.Threading.Tasks;

namespace Yugen.Home.Budget.Server.Components.Pages.Home;

public partial class Home
{
    protected override async Task OnInitializedAsync()
    {
        await ViewModel.LoadDataAsync();

        await base.OnInitializedAsync();
    }
}