using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Services;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models.Authentication;
using Yugen.HomeBudget.Shared.Models.Category;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Client.ViewModels.Expense;

internal sealed partial class ExpenseAddEditViewModel : ObservableObject
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
    private Yugen.HomeBudget.Client.Models.Expense _expense = new();

    [ObservableProperty]
    private List<ResponseCategoryDto> _categories = [];

    [ObservableProperty]
    private CurrentUser? _currentUser;

    private int? _id;

    public ExpenseAddEditViewModel(
        HttpClient httpClient,
        NavigationManager navigationManager,
        CustomStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _authStateProvider = authStateProvider;
    }

    public ResponseCategoryDto? CurrentCategory => Categories.FirstOrDefault(x => x.Id == Expense.CategoryId);

    public async Task OnInitializedAsync(int? id)
    {
        _id = id;

        Busy = true;

        CurrentUser = await _authStateProvider.GetCurrentUser();

        try
        {
            Categories = await _httpClient.GetFromJsonAsync<List<ResponseCategoryDto>>($"{EndpointConstants.Category}/all") ?? [];
            if (_id != null)
            {
                await LoadAsync();
            }
            else
            {
                Expense = new Models.Expense();
            }
        }
        finally
        {
            Busy = false;
        }
    }

    public void CategoryChanged()
    {
        if (CurrentCategory != null)
        {
            Expense.SubCategoryId = CurrentCategory.SubCategoriesDto.First().Id;
        }
    }

    public void CancelAsync()
    {
        Busy = true;
        _navigationManager.NavigateTo(PageConstants.ExpenseUrl);
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
                var createExpenseDto = new CreateExpenseDto(Expense.Title, Expense.Amount, Expense.DateTimeOffset, Expense.CategoryId, Expense.SubCategoryId, Expense.Accrued, CurrentUser?.Id, CurrentUser?.Id);
                var response = await _httpClient.PutAsJsonAsync<CreateExpenseDto>($"{EndpointConstants.Expense}/{_id}", createExpenseDto);
            }
            else
            {
                var updateExpenseDto = new UpdateExpenseDto(Expense.Id, Expense.Title, Expense.Amount, Expense.DateTimeOffset, Expense.CategoryId, Expense.SubCategoryId, Expense.Accrued, CurrentUser?.Id, CurrentUser?.Id);
                var response = await _httpClient.PostAsJsonAsync<UpdateExpenseDto>($"{EndpointConstants.Expense}", updateExpenseDto);
            }

            _navigationManager.NavigateTo(PageConstants.ExpenseUrl);
        }
        catch (Exception ex)
        {
            Error = true;
            ErrorMessage = ex.Message;
            Busy = false;
        }
    }

    private async Task LoadAsync()
    {
        Expense = new Models.Expense();
        var expenseDto = await _httpClient.GetFromJsonAsync<ResponseExpenseDto>($"{EndpointConstants.Expense}/{_id}");
        if (expenseDto != null)
        {
            Expense = new(expenseDto.Id, expenseDto.Title, expenseDto.Amount, expenseDto.DateTimeOffset, expenseDto.CategoryDto.Id, expenseDto.SubCategoryDto.Id, expenseDto.Accrued);
        }
    }
}