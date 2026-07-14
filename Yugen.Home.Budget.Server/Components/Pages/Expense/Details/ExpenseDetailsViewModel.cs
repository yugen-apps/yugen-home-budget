using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Models;
using Yugen.Common.Blazor.Account.Services;
using Yugen.Home.Budget.Application.Models.Expense;
using Yugen.Home.Budget.Application.Services;
using Yugen.Home.Budget.Server.Models.Expense;
using Yugen.Home.Budget.Server.Models.Navigation;

namespace Yugen.Home.Budget.Server.ViewModels.Expense;

public sealed partial class ExpenseDetailsViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly CategoryService _categoryService;
    private readonly ExpenseService _expenseService;
    private readonly NavigationManager _navigationManager;

    [ObservableProperty]
    public partial string Error { get; set; }
    [ObservableProperty]
    public partial ExpenseModel Model { get; set; }

    private bool _busy;
    private CurrentUser _currentUser;
    private int? _id;

    public bool IsAlertVisible;
    public bool IsFormValid;

    public ExpenseDetailsViewModel(
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

    public async Task OnInitializedAsync(int? id)
    {
        _id = id;

        _busy = true;

        _currentUser = await _authService.GetCurrentUserAsync();

        var categories = await _categoryService.ListAsync();

        try
        {
            if (_id != null)
            {
                var expenseDto = await _expenseService.GetAsync(_id ?? 0);
                if (expenseDto != null)
                {
                    Model = new(
                        categories,
                        expenseDto.Title,
                        expenseDto.Amount,
                        expenseDto.DateTimeOffset,
                        expenseDto.CategoryDto.Id,
                        expenseDto.SubCategoryDto.Id,
                        expenseDto.Accrued);
                }
            }
            else
            {
                Model = new(categories);
            }
        }
        finally
        {
            _busy = false;
        }
    }

    public void CategoryChanged()
    {
        Model.SubCategories = null;
        OnPropertyChanged(nameof(Model.SubCategories));

        Model.SubCategories = Model.SelectedCategory.SubCategoriesDto;
        Model.SelectedSubCategory = Model.SubCategories.First();
    }

    public void CancelAsync()
    {
        _busy = true;
        _navigationManager.NavigateTo(MenuConstants.ExpensePath);
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
                    var updateExpenseDto = new UpdateExpenseDto(
                        (int)_id,
                        Model.Title,
                        (decimal)Model.Amount,
                        (DateTime)Model.DateTime,
                        Model.SelectedCategory.Id,
                        Model.SelectedSubCategory.Id,
                        Model.Accrued,
                        _currentUser.Id,
                        _currentUser.Id);

                    var response = await _expenseService.UpdateAsync((int)_id, updateExpenseDto);
                }
                else
                {
                    var createExpenseDto = new CreateExpenseDto(
                        Model.Title,
                        (decimal)Model.Amount,
                        (DateTime)Model.DateTime,
                        Model.SelectedCategory.Id,
                        Model.SelectedSubCategory.Id,
                        Model.Accrued,
                        _currentUser.Id,
                        _currentUser.Id);

                    var response = await _expenseService.CreateAsync(createExpenseDto);
                }

                _navigationManager.NavigateTo(MenuConstants.ExpensePath);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            _busy = false;
        }
        IsAlertVisible = !string.IsNullOrWhiteSpace(Error);
    }
}