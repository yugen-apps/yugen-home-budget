using System.Threading.Tasks;

namespace Yugen.HomeBudget.Server.Components.Pages.Category.List;

public partial class CategoryList
{
    protected override async Task OnInitializedAsync()
    {
        await ViewModel.LoadDataAsync();

        await base.OnInitializedAsync();
    }
}