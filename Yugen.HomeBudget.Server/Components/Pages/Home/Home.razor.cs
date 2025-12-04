using System.Threading.Tasks;

namespace Yugen.HomeBudget.Server.Components.Pages.Home;

public partial class Home
{
    protected override async Task OnInitializedAsync()
    {
        await ViewModel.LoadDataAsync();

        await base.OnInitializedAsync();
    }
}