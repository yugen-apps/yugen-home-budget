using CommunityToolkit.Mvvm.ComponentModel;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Components;
using Yugen.Common.Blazor.Components.LoadingSpinner;
using Yugen.Common.Blazor.Models;
using Yugen.Home.Budget.Application.Models;
using Yugen.Home.Budget.Application.Models.Category;
using Yugen.Home.Budget.Application.Services;

namespace Yugen.Home.Budget.Server.ViewModels.Category;

public sealed partial class CategoryListViewModel : ObservableObject
{
    private readonly CategoryService _categoryService;
    private readonly IDialogService _dialogService;
    private readonly ILoadingSpinnerService _loadingSpinnerService;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }
    [ObservableProperty]
    public partial PaginatedList<ResponseCategoryDto> PaginatedList { get; set; } = new();

    [ObservableProperty]
    public partial int? PageNumber { get; set; } = 1;

    public CategoryListViewModel(
        CategoryService categoryService,
        IDialogService dialogService,
        ILoadingSpinnerService loadingSpinnerService)
    {
        _categoryService = categoryService;
        _dialogService = dialogService;
        _loadingSpinnerService = loadingSpinnerService;
    }

    public ICollection<ResponseCategoryDto> Categories => PaginatedList.Items;

    public async Task LoadDataAsync()
    {
        IsLoading = true;
        _loadingSpinnerService.Wait();

        try
        {
            PaginatedList = await _categoryService.ListAsync(PageNumber ?? 0, Constants.PageSizeSmall) ?? new();
        }
        finally
        {
            IsLoading = false;
            _loadingSpinnerService.Resume();
        }
    }

    public async Task PageIndexChanged(int newPageNumber)
    {
        if (newPageNumber < 1 ||
            newPageNumber > PaginatedList.TotalPages)
        {
            return;
        }

        PageNumber = newPageNumber;
        await LoadDataAsync();
    }

    public async Task ShowDeleteConfirmMessage(int id)
    {
        var parameters = new DialogParameters<MessageDialog>
        {
            { x => x.ContentText, "Do you really want to delete these records? This process cannot be undone." },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var dialog = await _dialogService.ShowAsync<MessageDialog>("Delete", parameters);

        var result = await dialog.Result;
        if ((result?.Canceled) != false)
        {
            return;
        }

        await DeleteAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var result = await _categoryService.DeleteAsync(id);
        if (result)
        {
            var category = Categories?.FirstOrDefault(c => c.Id.Equals(id));
            if (category != null)
            {
                Categories?.Remove(category);
            }
        }
    }
}
