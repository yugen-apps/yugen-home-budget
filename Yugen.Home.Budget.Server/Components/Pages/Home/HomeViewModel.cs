using CommunityToolkit.Mvvm.ComponentModel;
using MudBlazor;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Home.Budget.Application.Models;
using Yugen.Home.Budget.Application.Models.Expense;
using Yugen.Home.Budget.Application.Services;
using Yugen.Home.Budget.Server.Components.Shared;
using Yugen.Home.Budget.Server.Models.Home;
using Yugen.Common.Blazor.Components;
using Yugen.Common.Blazor.Components.LoadingSpinner;

namespace Yugen.Home.Budget.Server.ViewModels;

public sealed partial class HomeViewModel : ObservableObject
{
    private readonly CategoryService _categoryService;
    private readonly ExpenseService _expenseService;
    private readonly IDialogService _dialogService;
    private readonly ILoadingSpinnerService _loadingSpinnerService;

    private readonly TaskCompletionSource _tcs = new();
    private readonly DateTimeOffset _now = DateTimeOffset.UtcNow;

    [ObservableProperty]
    private TotalExpense _currentYearTotalExpense;

    [ObservableProperty]
    private TotalExpense _previousMonthTotalExpense;

    [ObservableProperty]
    private TotalExpense _currentMonthTotalExpense;

    [ObservableProperty]
    private TotalAccrued _currentYearTotalAccrued;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isDataGridLoading;

    [ObservableProperty]
    private PaginatedList<ResponseExpenseDto> _paginatedList = new();

    public HomeViewModel(
        CategoryService categoryService,
        ExpenseService expenseService,
        IDialogService dialogService,
        ILoadingSpinnerService loadingSpinnerService)
    {
        _categoryService = categoryService;
        _expenseService = expenseService;
        _dialogService = dialogService;
        _loadingSpinnerService = loadingSpinnerService;
    }

    private int Year => _now.Year;

    private int Month => _now.Month;

    private int PreviousMonth => _now.AddMonths(-1).Month;

    public string[] Labels { get; private set; } = [];

    public double[] Data { get; private set; } = [];
        
    public async Task LoadDataAsync()
    {
        _loadingSpinnerService.Wait();
        IsLoading = true;

        await LoadCurrentYearTotalExpense();
        await LoadPreviousMonthTotalExpense();
        await LoadCurrentMonthTotalExpense();
        await LoadCurrentYearTotalAccrued();
        await LoadChartData();

        _tcs.SetResult();

        IsLoading = false;
        _loadingSpinnerService.Resume();
    }

    public async Task LoadCurrentYearTotalExpense()
    {
        try
        {
            var currentYearTotalExpense = await _expenseService.SumAsync(Year, 0);
            CurrentYearTotalExpense = new TotalExpense(currentYearTotalExpense);
        }
        catch { }
    }

    public async Task LoadPreviousMonthTotalExpense()
    {
        try
        {
            var previousMonthExpenseTotal = await _expenseService.SumAsync(Year, PreviousMonth);
            PreviousMonthTotalExpense = new TotalExpense(previousMonthExpenseTotal, CurrentMonthTotalExpense?.Current ?? 0);
        }
        catch { }
    }

    public async Task LoadCurrentMonthTotalExpense()
    {
        try
        {
            var currentMonthExpenseTotal = await _expenseService.SumAsync(Year, Month);
            CurrentMonthTotalExpense = new TotalExpense(currentMonthExpenseTotal, PreviousMonthTotalExpense?.Current ?? 0);
        }
        catch { }
    }

    public async Task LoadCurrentYearTotalAccrued()
    {
        try
        {
            var currentYearTotalAccrued = await _expenseService.SumAccruedAsync(Year, 0);
            CurrentYearTotalAccrued = new TotalAccrued(currentYearTotalAccrued);
        }
        catch { }
    }

    public async Task LoadChartData()
    {
        try
        {
            var data = await _expenseService.GroupedByCategoryAsync(Year, Month);
            if (data != null)
            {
                var labels = data.Select(x => x.Category).ToList();
                var values = data.Select(x => (double)x.Total).ToList();

                var currentMonthAccruedTotal = await _expenseService.SumAccruedAsync(Year, Month);
                labels.Add("Accrued");
                values.Add((double)currentMonthAccruedTotal);

                Labels = labels.ToArray();
                Data = values.ToArray();
            }
        }
        catch { }
    }

    public async Task<GridData<ResponseExpenseDto>> ServerReload(GridState<ResponseExpenseDto> state, CancellationToken cancellationToken)
    {
        try
        {
            IsDataGridLoading = true;

            await _tcs.Task;

            PaginatedList = await _expenseService.ListAsync(state.Page, state.PageSize, Month, Year) ?? new();

            return new GridData<ResponseExpenseDto>
            {
                TotalItems = PaginatedList.TotalItems,
                Items = PaginatedList.Items
            };
        }
        finally
        {
            IsDataGridLoading = false;
        }
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

    private async Task DeleteAsync(int id)
    {
        var result = await _expenseService.DeleteAsync(id);
        if (!result)
        {
            return;
        }

        var deletedItem = PaginatedList.Items.FirstOrDefault(x => x.Id == id);
        if (deletedItem == null)
        {
            return;
        }

        PaginatedList.Remove(deletedItem);
    }
}