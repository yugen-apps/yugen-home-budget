using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Services;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models.Authentication;
using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Client.ViewModels.Category;

internal sealed partial class CategoryAddEditViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    private readonly CustomStateProvider _authStateProvider;

    [ObservableProperty]
    private bool _busy;

    [ObservableProperty]
    private bool _error;

    [ObservableProperty]
    private bool _concurrencyError;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _showPopup;

    [ObservableProperty]
    private Yugen.HomeBudget.Client.Models.Category? _category;

    [ObservableProperty]
    private string? _newSubCategoryTitle;

    [ObservableProperty]
    private CurrentUser? _currentUser;

    private int? _id;

    public CategoryAddEditViewModel(
            HttpClient httpClient,
        NavigationManager navigationManager,
        CustomStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _authStateProvider = authStateProvider;
    }

    public async Task OnInitializedAsync(int? id)
    {
        _id = id;

        Busy = true;

        CurrentUser = await _authStateProvider.GetCurrentUser();

        try
        {
            if (_id != null)
            {
                await LoadAsync();
            }
            else
            {
                Category = new(0, "");
            }
        }
        finally
        {
            Busy = false;
        }
    }

    public void CancelAsync()
    {
        Busy = true;
        _navigationManager.NavigateTo(PageConstants.CategoryUrl);
    }

    public async Task SubmitAsync(bool isValid)
    {
        if (Busy)
        {
            return;
        }

        if (!isValid)
        {
            Error = false;
            ConcurrencyError = false;
            return;
        }

        Busy = true;
        try
        {
            if (_id != null)
            {
                var createCategoryDto = new CreateCategoryDto(Category.Title, "", Category.SubCategoriesDto, CurrentUser?.Id, CurrentUser?.Id);
                var response = await _httpClient.PutAsJsonAsync<CreateCategoryDto>($"{EndpointConstants.Category}/{_id}", createCategoryDto);
                //var c = await response.Content.ReadFromJsonAsync<CategoryDto>();
            }
            else
            {
                var updateCategoryDto = new UpdateCategoryDto(Category.Id, Category.Title, "", Category.SubCategoriesDto, CurrentUser?.Id, CurrentUser?.Id);
                var response = await _httpClient.PostAsJsonAsync<UpdateCategoryDto>($"{EndpointConstants.Category}", updateCategoryDto);
            }

            _navigationManager.NavigateTo(PageConstants.CategoryUrl);
        }
        catch (Exception ex)
        {
            Error = true;
            ErrorMessage = ex.Message;
            Busy = false;
        }
    }

    public void DeleteSubCategoryAsync(SubCategoryDto subCategoryDto)
    {
        Category?.SubCategoriesDto.Remove(subCategoryDto);
    }

    public void ShowAddPopup()
    {
        NewSubCategoryTitle = string.Empty;
        ShowPopup = true;
    }

    public void ClosePopup()
    {
        ShowPopup = false;
    }

    public void AddSubCategory()
    {
        Category?.SubCategoriesDto.Add(new SubCategoryDto(0, NewSubCategoryTitle));
        ClosePopup();
    }

    private async Task LoadAsync()
    {
        Category = null;
        var categoryDto = await _httpClient.GetFromJsonAsync<ResponseCategoryDto>($"{EndpointConstants.Category}/{_id}");
        Category = new(categoryDto.Id, categoryDto.Title, categoryDto.SubCategoriesDto);
    }
}