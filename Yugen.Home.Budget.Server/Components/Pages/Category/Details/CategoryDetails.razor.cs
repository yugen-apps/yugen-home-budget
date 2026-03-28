using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;
using Yugen.Home.Budget.Application.Models.Category;

namespace Yugen.Home.Budget.Server.Components.Pages.Category.Details;

public partial class CategoryDetails
{
    [Parameter]
    public int? Id { get; set; }

    private MudDataGrid<SubCategoryDto> _elementGrid = default!;

    protected override async Task OnInitializedAsync()
    {
        await ViewModel.OnInitializedAsync(Id);

        await base.OnInitializedAsync();
    }

    private async Task NewItemAsync()
    {
        await _elementGrid.SetEditingItemAsync(new SubCategoryDto());
    }
}