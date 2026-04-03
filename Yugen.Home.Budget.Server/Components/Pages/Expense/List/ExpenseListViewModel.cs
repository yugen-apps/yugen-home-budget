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
using Yugen.Common.Blazor.Components;
using Yugen.Common.Blazor.Components.LoadingSpinner;

namespace Yugen.Home.Budget.Server.ViewModels.Expense;

public sealed partial class ExpenseListViewModel : ObservableObject
{
    public int[] Months = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
    public int[] Years = { 2023, 2024, 2025 };
    private readonly ExpenseService _expenseService;
    private readonly IDialogService _dialogService;
    private readonly ILoadingSpinnerService _loadingSpinnerService;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private PaginatedList<ResponseExpenseDto> _paginatedList = new();

    [ObservableProperty]
    private int _selectedMonth = DateTimeOffset.UtcNow.Month;

    [ObservableProperty]
    private int _selectedYear = DateTimeOffset.UtcNow.Year;

    public ExpenseListViewModel(
        ExpenseService expenseService,
        IDialogService dialogService,
        ILoadingSpinnerService loadingSpinnerService)
    {
        _expenseService = expenseService;
        _dialogService = dialogService;
        _loadingSpinnerService = loadingSpinnerService;
    }

    public Func<Task> RefreshServerDataFunc { get; set; }

    public async Task<GridData<ResponseExpenseDto>> ServerReload(GridState<ResponseExpenseDto> state, CancellationToken cancellationToken)
    {
        await RefreshDataAsync(state.Page, state.PageSize);

        return new GridData<ResponseExpenseDto>
        {
            TotalItems = PaginatedList.TotalItems,
            Items = PaginatedList.Items
        };
    }

    public Task DateChanged(int? month, int? year)
    {
        SelectedMonth = month ?? SelectedMonth;
        SelectedYear = year ?? SelectedYear;
        return RefreshServerDataFunc.Invoke();
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

    public async Task RefreshDataAsync(int page, int pageSize)
    {
        _loadingSpinnerService.Wait();
        IsLoading = true;

        try
        {
            PaginatedList = await _expenseService.ListAsync(page, pageSize, SelectedMonth, SelectedYear) ?? new();
        }
        finally
        {
            IsLoading = false;
            _loadingSpinnerService.Resume();
        }
    }
}