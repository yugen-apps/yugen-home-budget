using Blazorise;
using Blazorise.DataGrid;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Application.Services;
using Yugen.HomeBudget.Server.Navigation;
using Yugen.HomeBudget.Server.Services;
using Yugen.HomeBudget.Shared.Models.Authentication;
using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Server.ViewModels.Category;

public sealed partial class CategoryAddEditViewModel : ObservableObject
{
    public bool IsAlertVisible;
    public Validations validations = new();
    private readonly AuthService _authService;
    private readonly CategoryService _categoryService;
    private readonly NavigationManager _navigationManager;
    private readonly IMessageService _messageService;

    [ObservableProperty]
    private bool _busy;

    [ObservableProperty]
    private string? _error;

    [ObservableProperty]
    private bool _showIconsPopup;

    [ObservableProperty]
    private Server.Models.Category _category = new();

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
        AuthService authService,
        CategoryService categoryService,
        NavigationManager navigationManager,
        IMessageService messageService)
    {
        _authService = authService;
        _categoryService = categoryService;
        _navigationManager = navigationManager;
        _messageService = messageService;
    }

    public async Task OnInitializedAsync(int? id)
    {
        _id = id;

        Busy = true;

        CurrentUser = await _authService.GetCurrentUserAsync();

        try
        {
            if (_id != null)
            {
                await LoadAsync();
            }
            else
            {
                Category = new Yugen.HomeBudget.Server.Models.Category();
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
        _navigationManager.NavigateTo(MenuConstants.CategoriesPath);
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
                    var updateCategoryDto = new UpdateCategoryDto(Category.Id, Category.Title, Category.IconString, Category.SubCategoriesDto, CurrentUser?.Id, CurrentUser?.Id);
                    var response = await _categoryService.UpdateAsync((int)_id, updateCategoryDto);
                    //var response = await _httpClient.PutAsJsonAsync<CreateCategoryDto>($"{EndpointConstants.Category}/{_id}", createCategoryDto);
                    //var c = await response.Content.ReadFromJsonAsync<CategoryDto>();
                }
                else
                {
                    var createCategoryDto = new CreateCategoryDto(Category.Title, Category.IconString, Category.SubCategoriesDto, CurrentUser?.Id, CurrentUser?.Id);
                    var response = await _categoryService.CreateAsync(createCategoryDto);
                    //var response = await _httpClient.PostAsJsonAsync<UpdateCategoryDto>($"{EndpointConstants.Category}", updateCategoryDto);
                }

                _navigationManager.NavigateTo(MenuConstants.CategoriesPath);
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
        //var categoryDto = await _httpClient.GetFromJsonAsync<ResponseCategoryDto>($"{EndpointConstants.Category}/{_id}");
        var categoryDto = await _categoryService.GetAsync(_id ?? 0);
        if (categoryDto != null)
        {
            Category = new Yugen.HomeBudget.Server.Models.Category(categoryDto.Id, categoryDto.Title, categoryDto.Icon, categoryDto.SubCategoriesDto);
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