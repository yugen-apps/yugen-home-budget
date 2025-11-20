using Blazorise;
using Blazorise.DataGrid;
using CommunityToolkit.Mvvm.ComponentModel;
using Yugen.HomeBudget.Application.Services;
using Yugen.HomeBudget.Server.Models;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Server.ViewModels.Expense;

public sealed partial class ExpenseListViewModel : ObservableObject
{
    public int[] Months = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
    public int[] Years = { 2023, 2024, 2025 };
    private readonly ExpenseService _expenseService;
    private readonly IMessageService _messageService;

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
        IMessageService messageService)
    {
        _expenseService = expenseService;
        _messageService = messageService;
    }

    public async Task DateChanged(int? month, int? year)
    {
        SelectedMonth = month ?? SelectedMonth;
        SelectedYear = year ?? SelectedYear;
        await RefreshDataAsync(1, Constants.PageSizeSmall);
    }

    public async Task OnReadData(int page, int pageSize)
    {
        await RefreshDataAsync(page, pageSize);
    }

    public async Task OnRowRemoving(CancellableRowChange<ResponseExpenseDto> e)
    {
        e.Cancel = await ShowDeleteConfirmMessage(e.OldItem.Id);
    }

    public async Task<bool> ShowDeleteConfirmMessage(int id)
    {
        var confirmed = await _messageService.Confirm("Are you sure you want to delete?", "Confirmation");
        if (confirmed)
        {
            return await DeleteAsync(id);
        }
        return true;
    }

    private async Task<bool> DeleteAsync(int id)
    {
        //var result = await _httpClient.DeleteAsync($"{EndpointConstants.Expense}/{id}");
        var result = await _expenseService.DeleteAsync(id);
        if (result)
        {
            return false;
        }
        return true;
    }

    private async Task RefreshDataAsync(int page, int pageSize)
    {
        IsLoading = true;

        try
        {
            PaginatedList = await _expenseService.ListAsync(SelectedYear, SelectedMonth, page, Constants.PageSize) ?? new();
            //var response = await _httpClient.GetFromJsonAsync<PaginatedList<ResponseExpenseDto>>($"{EndpointConstants.Expense}?year={SelectedYear}&month={SelectedMonth}&pageNumber={page}&pageSize={pageSize}");
            //PaginatedList = response ?? new();
        }
        finally
        {
            IsLoading = false;
        }
    }
}