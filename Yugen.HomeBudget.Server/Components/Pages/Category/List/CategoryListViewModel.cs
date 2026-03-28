using CommunityToolkit.Mvvm.ComponentModel;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yugen.HomeBudget.Application.Models;
using Yugen.HomeBudget.Application.Models.Category;
using Yugen.HomeBudget.Application.Services;
using Yugen.HomeBudget.Server.Components.Shared;
using Yugen.HomeBudget.Server.Components.Shared.LoadingSpinner;
using Yugen.HomeBudget.Server.Models;
using Yugen.Shared.Components;
using Yugen.Shared.Models;

namespace Yugen.HomeBudget.Server.ViewModels.Category;

public sealed partial class CategoryListViewModel : ObservableObject
{
    private readonly CategoryService _categoryService;
    private readonly IDialogService _dialogService;
    private readonly ILoadingSpinnerService _loadingSpinnerService;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private PaginatedList<ResponseCategoryDto> _paginatedList = new();

    [ObservableProperty]
    private int? _pageNumber = 1;

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
