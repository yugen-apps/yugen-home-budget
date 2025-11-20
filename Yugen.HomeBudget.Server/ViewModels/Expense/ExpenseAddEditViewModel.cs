using Blazorise;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Application.Services;
using Yugen.HomeBudget.Server.Navigation;
using Yugen.HomeBudget.Server.Services;
using Yugen.HomeBudget.Shared.Models.Authentication;
using Yugen.HomeBudget.Shared.Models.Category;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Server.ViewModels.Expense;

public sealed partial class ExpenseAddEditViewModel : ObservableObject
{
    public bool IsAlertVisible;
    public Validations validations = new();
    private readonly AuthService _authService;
    private readonly CategoryService _categoryService;
    private readonly ExpenseService _expenseService;
    private readonly NavigationManager _navigationManager;

    [ObservableProperty]
    private bool _busy;

    [ObservableProperty]
    private string? _error;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _showPopup;

    [ObservableProperty]
    private Server.Models.Expense _expense = new();

    [ObservableProperty]
    private List<ResponseCategoryDto> _categories = [];

    [ObservableProperty]
    private CurrentUser? _currentUser;

    private int? _id;

    public ExpenseAddEditViewModel(
        AuthService authService,
        CategoryService categoryService,
        ExpenseService expenseService,
        NavigationManager navigationManager)
    {
        _authService = authService;
        _categoryService = categoryService;
        _expenseService = expenseService;
        _navigationManager = navigationManager;
    }

    public ResponseCategoryDto? CurrentCategory => Categories.FirstOrDefault(x => x.Id == Expense.CategoryId);

    public async Task OnInitializedAsync(int? id)
    {
        _id = id;

        Busy = true;

        CurrentUser = await _authService.GetCurrentUserAsync();

        try
        {
            Categories = (await _categoryService.ListAsync()).ToList();
            //Categories = await _httpClient.GetFromJsonAsync<List<ResponseCategoryDto>>($"{EndpointConstants.Category}/all") ?? [];
            if (_id != null)
            {
                await LoadAsync();
            }
            else
            {
                Expense = new Yugen.HomeBudget.Server.Models.Expense();
            }
        }
        finally
        {
            Busy = false;
        }
    }

    public void CategoryChanged(int id)
    {
        Expense.CategoryId = id;
        if (CurrentCategory != null)
        {
            Expense.SubCategoryId = CurrentCategory.SubCategoriesDto.First().Id;
        }
    }

    public void CancelAsync()
    {
        Busy = true;
        _navigationManager.NavigateTo(MenuConstants.ExpensesPath);
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
                    var updateExpenseDto = new UpdateExpenseDto(Expense.Id, Expense.Title, Expense.Amount, Expense.DateTimeOffset, Expense.CategoryId, Expense.SubCategoryId, Expense.Accrued, CurrentUser?.Id, CurrentUser?.Id);
                    //var response = await _httpClient.PutAsJsonAsync<CreateExpenseDto>($"{EndpointConstants.Expense}/{_id}", createExpenseDto);
                    var response = await _expenseService.UpdateAsync((int)_id, updateExpenseDto);

                }
                else
                {
                    var createExpenseDto = new CreateExpenseDto(Expense.Title, Expense.Amount, Expense.DateTimeOffset, Expense.CategoryId, Expense.SubCategoryId, Expense.Accrued, CurrentUser?.Id, CurrentUser?.Id);
                    //var response = await _httpClient.PostAsJsonAsync<UpdateExpenseDto>($"{EndpointConstants.Expense}", updateExpenseDto);
                    var response = await _expenseService.CreateAsync(createExpenseDto);

                }

                _navigationManager.NavigateTo(MenuConstants.ExpensesPath);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Busy = false;
            }
        }
        IsAlertVisible = !string.IsNullOrWhiteSpace(Error);
    }

    private async Task LoadAsync()
    {
        Expense = new Yugen.HomeBudget.Server.Models.Expense();
        //var expenseDto = await _httpClient.GetFromJsonAsync<ResponseExpenseDto>($"{EndpointConstants.Expense}/{_id}");
        var expenseDto = await _expenseService.GetAsync(_id ?? 0);
        if (expenseDto != null)
        {
            Expense = new(expenseDto.Id, expenseDto.Title, expenseDto.Amount, expenseDto.DateTimeOffset, expenseDto.CategoryDto.Id, expenseDto.SubCategoryDto.Id, expenseDto.Accrued);
        }
    }
}