using System.Threading.Tasks;

namespace Yugen.Home.Budget.Server.Components.Pages.Category.List;

public partial class CategoryList
{
    protected override async Task OnInitializedAsync()
    {
        await ViewModel.LoadDataAsync();

        await base.OnInitializedAsync();
    }
}