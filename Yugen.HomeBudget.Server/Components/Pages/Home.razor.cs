using System.Threading.Tasks;

namespace Yugen.HomeBudget.Server.Components.Pages
{
    public partial class Home
    {
        protected override async Task OnInitializedAsync()
        {
            await ViewModel.LoadDataAsync();

            await base.OnInitializedAsync();
        }
    }
}