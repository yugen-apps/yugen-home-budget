using CommunityToolkit.Mvvm.ComponentModel;
using MudBlazor;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Components;
using Yugen.Common.Blazor.Components.LoadingSpinner;
using Yugen.Home.Budget.Application.Models;
using Yugen.Home.Budget.Application.Models.Expense;
using Yugen.Home.Budget.Application.Services;
using Yugen.Home.Budget.Server.Helpers;
using Yugen.Home.Budget.Server.Models.Home;

namespace Yugen.Home.Budget.Server.ViewModels;

public sealed partial class HomeViewModel : ObservableObject
{
    private readonly CategoryService _categoryService;
    private readonly ExpenseService _expenseService;
    private readonly IDialogService _dialogService;
    private readonly ILoadingSpinnerService _loadingSpinnerService;

    private readonly TaskCompletionSource _tcs = new();

    [ObservableProperty]
    public partial TotalExpense MonthTotalExpense { get; set; }

    [ObservableProperty]
    public partial TotalAccrued MonthTotalAccrued { get; set; }

    [ObservableProperty]
    public partial TotalExpense YearTotalExpense { get; set; }

    [ObservableProperty]
    public partial TotalAccrued YearTotalAccrued { get; set; }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool IsDataGridLoading { get; set; }

    [ObservableProperty]
    public partial PaginatedList<ResponseExpenseDto> PaginatedList { get; set; } = new();

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

    public string[] Labels { get; private set; } = [];

    public double[] Data { get; private set; } = [];

    public async Task LoadDataAsync()
    {
        _loadingSpinnerService.Wait();
        IsLoading = true;

        try
        {
            var previousMonthExpenseTotal = await _expenseService.SumAsync(DateTimeHelper.PreviousMonthStart, DateTimeHelper.PreviousMonthEnd);
            var currentMonthExpenseTotal = await _expenseService.SumAsync(DateTimeHelper.CurrentMonthStart, DateTimeHelper.CurrentMonthEnd);
            MonthTotalExpense = new TotalExpense(currentMonthExpenseTotal, previousMonthExpenseTotal);

            var previousMonthTotalAccrued = await _expenseService.SumAccruedAsync(DateTimeHelper.PreviousMonthStart, DateTimeHelper.PreviousMonthEnd);
            var currentMonthTotalAccrued = await _expenseService.SumAccruedAsync(DateTimeHelper.CurrentMonthStart, DateTimeHelper.CurrentMonthEnd);
            MonthTotalAccrued = new TotalAccrued(currentMonthTotalAccrued, previousMonthTotalAccrued);

            var previousYearTotalExpense = await _expenseService.SumAsync(DateTimeHelper.PreviousYearStart, DateTimeHelper.PreviousYearEnd);
            var currentYearTotalExpense = await _expenseService.SumAsync(DateTimeHelper.CurrentYearStart, DateTimeHelper.CurrentYearEnd);
            YearTotalExpense = new TotalExpense(currentYearTotalExpense, previousYearTotalExpense);

            var previousYearTotalAccrued = await _expenseService.SumAccruedAsync(DateTimeHelper.PreviousYearStart, DateTimeHelper.PreviousYearEnd);
            var currentYearTotalAccrued = await _expenseService.SumAccruedAsync(DateTimeHelper.CurrentYearStart, DateTimeHelper.CurrentYearEnd);
            YearTotalAccrued = new TotalAccrued(currentYearTotalAccrued, previousYearTotalAccrued);
        }
        catch { }

        await LoadChartData();

        _tcs.SetResult();

        IsLoading = false;
        _loadingSpinnerService.Resume();
    }

    public async Task LoadChartData()
    {
        try
        {
            var data = await _expenseService.GroupedByCategoryAsync(DateTimeHelper.CurrentMonthStart, DateTimeHelper.CurrentMonthEnd);
            if (data != null)
            {
                var labels = data.Select(x => x.Category).ToList();
                var values = data.Select(x => (double)x.Total).ToList();

                var currentMonthAccruedTotal = await _expenseService.SumAccruedAsync(DateTimeHelper.CurrentMonthStart, DateTimeHelper.CurrentMonthEnd);
                labels.Add("Accrued");
                values.Add((double)currentMonthAccruedTotal);

                Labels = labels.ToArray();
                Data = values.ToArray();
            }
        }
        catch { }
    }

    public async Task<GridData<ResponseExpenseDto>> ServerReload(GridState<ResponseExpenseDto> state, CancellationToken _)
    {
        try
        {
            IsDataGridLoading = true;

            await _tcs.Task;

            PaginatedList = await _expenseService.ListAsync(state.Page, state.PageSize, DateTimeHelper.CurrentMonthStart, DateTimeHelper.CurrentMonthEnd) ?? new();

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