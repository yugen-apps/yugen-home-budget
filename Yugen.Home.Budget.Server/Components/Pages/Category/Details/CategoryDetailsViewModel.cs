using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Models;
using Yugen.Common.Blazor.Account.Services;
using Yugen.Common.Blazor.Components;
using Yugen.Home.Budget.Application.Models.Category;
using Yugen.Home.Budget.Application.Services;
using Yugen.Home.Budget.Server.Components.Shared;
using Yugen.Home.Budget.Server.Models.Category;
using Yugen.Home.Budget.Server.Models.Navigation;

namespace Yugen.Home.Budget.Server.ViewModels.Category;

public sealed partial class CategoryDetailsViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly CategoryService _categoryService;
    private readonly NavigationManager _navigationManager;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    public partial string Error { get; set; }

    private bool _busy;
    private CurrentUser _currentUser;
    private int? _id;

    public bool IsAlertVisible;
    public bool IsFormValid;
    public CategoryModel Model;

    public CategoryDetailsViewModel(
        AuthService authService,
        CategoryService categoryService,
        NavigationManager navigationManager,
        IDialogService dialogService)
    {
        _authService = authService;
        _categoryService = categoryService;
        _navigationManager = navigationManager;
        _dialogService = dialogService;
    }

    public async Task OnInitializedAsync(int? id)
    {
        _id = id;

        _busy = true;

        _currentUser = await _authService.GetCurrentUserAsync();

        try
        {
            if (_id != null)
            {
                var categoryDto = await _categoryService.GetAsync(_id ?? 0);
                if (categoryDto != null)
                {
                    Model = new CategoryModel(
                        categoryDto.Id,
                        categoryDto.Title,
                        categoryDto.IconName,
                        categoryDto.SubCategoriesDto);
                }
            }
            else
            {
                Model = new CategoryModel();
            }
        }
        finally
        {
            _busy = false;
        }
    }

    public void CancelAsync()
    {
        _busy = true;
        _navigationManager.NavigateTo(MenuConstants.CategoryPath);
    }

    public async Task OnSubmit()
    {
        if (_busy)
        {
            return;
        }

        Error = null;

        if (IsFormValid)
        {
            try
            {
                _busy = true;
                if (_id != null)
                {
                    var updateCategoryDto = new UpdateCategoryDto(Model.Id, Model.Title, Model.IconName, Model.SubCategoriesDto, _currentUser.Id, _currentUser.Id);
                    var response = await _categoryService.UpdateAsync((int)_id, updateCategoryDto);
                }
                else
                {
                    var createCategoryDto = new CreateCategoryDto(Model.Title, Model.IconName, Model.SubCategoriesDto, _currentUser.Id, _currentUser.Id);
                    var response = await _categoryService.CreateAsync(createCategoryDto);
                }

                _navigationManager.NavigateTo(MenuConstants.CategoryPath);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                _busy = false;
            }
        }
        IsAlertVisible = !string.IsNullOrWhiteSpace(Error);
    }

    public async Task ShowIconsDialog(string icon)
    {
        var parameters = new DialogParameters<IconsDialog>
        {
            { x => x.ButtonText, "Save" },
            { x => x.IconName, icon }
        };

        var dialog = await _dialogService.ShowAsync<IconsDialog>("Add/Edit Icon", parameters);

        var result = await dialog.Result;
        if ((result?.Canceled) != false)
        {
            return;
        }

        var newIcon = result.Data as string;
        Model.IconName = newIcon ?? Icons.Material.Filled.FormatBold;
    }

    public Task<DataGridEditFormAction> CommittedItemChanges(SubCategoryDto item)
    {
        if (item.Id == 0)
        {
            Model.SubCategoriesDto.Add(item);
        }

        return Task.FromResult(DataGridEditFormAction.Close);
    }

    public async Task ShowDeleteConfirmMessage(SubCategoryDto subCategoryDto)
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

        Model?.SubCategoriesDto.Remove(subCategoryDto);
    }
}