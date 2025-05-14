using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Services;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models.Authentication;
using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Client.ViewModels.Category;

public sealed partial class CategoryAddEditViewModel : ObservableObject
{
    public bool IsAlertVisible;
    public Validations validations = new();
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    private readonly CustomStateProvider _authStateProvider;
    private readonly IMessageService _messageService;

    [ObservableProperty]
    private bool _busy;

    [ObservableProperty]
    private string? _error;

    [ObservableProperty]
    private bool _showIconsPopup;

    [ObservableProperty]
    private Models.Category _category = new();

    [ObservableProperty]
    private string _newSubCategoryTitle = string.Empty;

    [ObservableProperty]
    private IconName? _newIcon;

    [ObservableProperty]
    private CurrentUser? _currentUser;

    [ObservableProperty]
    private IconName[] _iconList = (IconName[])Enum.GetValues(typeof(IconName));

    private int? _id;

    public CategoryAddEditViewModel(
        HttpClient httpClient,
        NavigationManager navigationManager,
        CustomStateProvider authStateProvider,
        IMessageService messageService)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _authStateProvider = authStateProvider;
        _messageService = messageService;
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
                Category = new Models.Category();
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

    public async Task OnSubmit()
    {
        if (Busy)
        {
            return;
        }

        Error = null;
        if (await validations.ValidateAll())
        {
            try
            {
                await validations.ClearAll();
                Busy = true;
                if (_id != null)
                {
                    var createCategoryDto = new CreateCategoryDto(Category.Title, Category.IconString, Category.SubCategoriesDto, CurrentUser?.Id, CurrentUser?.Id);
                    var response = await _httpClient.PutAsJsonAsync<CreateCategoryDto>($"{EndpointConstants.Category}/{_id}", createCategoryDto);
                    //var c = await response.Content.ReadFromJsonAsync<CategoryDto>();
                }
                else
                {
                    var updateCategoryDto = new UpdateCategoryDto(Category.Id, Category.Title, Category.IconString, Category.SubCategoriesDto, CurrentUser?.Id, CurrentUser?.Id);
                    var response = await _httpClient.PostAsJsonAsync<UpdateCategoryDto>($"{EndpointConstants.Category}", updateCategoryDto);
                }

                _navigationManager.NavigateTo(PageConstants.CategoryUrl);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Busy = false;
            }
        }
        IsAlertVisible = !string.IsNullOrWhiteSpace(Error);
    }

    public void OpenIconsPopup()
    {
        NewIcon = null;
        ShowIconsPopup = true;
    }

    public void CloseIconsPopup()
    {
        ShowIconsPopup = false;
    }

    public void AddIcon(IconName icon)
    {
        NewIcon = icon;
    }

    public void SaveIcon()
    {
        Category.Icon = NewIcon ?? IconName.Bold;
        CloseIconsPopup();
    }

    public Color IsActive(IconName icon)
    {
        return NewIcon == icon ? Color.Primary : Color.Default;
    }

    private async Task LoadAsync()
    {
        var categoryDto = await _httpClient.GetFromJsonAsync<ResponseCategoryDto>($"{EndpointConstants.Category}/{_id}");
        if (categoryDto != null)
        {
            Category = new Models.Category(categoryDto.Id, categoryDto.Title, categoryDto.Icon, categoryDto.SubCategoriesDto);
        }
    }

    public async Task OnRowRemoving(CancellableRowChange<SubCategoryDto> e)
    {
        e.Cancel = await ShowDeleteConfirmMessage(e.OldItem);
    }

    public async Task<bool> ShowDeleteConfirmMessage(SubCategoryDto subCategoryDto)
    {
        var confirmed = await _messageService.Confirm("Are you sure you want to delete?", "Confirmation");
        if (confirmed)
        {
            Category?.SubCategoriesDto.Remove(subCategoryDto);
            return false;
        }
        return true;
    }
}