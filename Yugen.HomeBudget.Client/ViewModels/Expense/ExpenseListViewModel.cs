using Blazorise;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Models;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Client.ViewModels.Expense;

public sealed partial class ExpenseListViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;
    private readonly IMessageService _messageService;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private PaginatedList<ResponseExpenseDto> _paginatedList = new();

    [ObservableProperty]
    private int? _pageNumber = 1;

    [ObservableProperty]
    private int _selectedYear = DateTimeOffset.UtcNow.Year;

    [ObservableProperty]
    private int _selectedMonth = DateTimeOffset.UtcNow.Month;

    public ExpenseListViewModel(
        HttpClient httpClient, 
        IMessageService messageService)
    {
        _httpClient = httpClient;
        _messageService = messageService;
    }

    public int[] Months = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

    public int[] Years = { 2023, 2024, 2025 };

    public async Task LoadDataAsync()
    {
        IsLoading = true;

        try
        {
            var response = await _httpClient.GetFromJsonAsync<PaginatedList<ResponseExpenseDto>>($"{EndpointConstants.Expense}?year={SelectedYear}&month={SelectedMonth}&pageNumber={PageNumber}&pageSize={Constants.PageSize}");
            if (response != null)
            {
                PaginatedList = response;
            }
        }
        finally
        {
            IsLoading = false;
        }
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
        var result = await _httpClient.DeleteAsync($"{EndpointConstants.Expense}/{id}");
        if (result.IsSuccessStatusCode)
        {
            return false;
        }
        return true;
    }

    public async Task DateChanged(int? month, int? year)
    {
        SelectedMonth = month ?? SelectedMonth;
        SelectedYear = year ?? SelectedYear;
        await LoadDataAsync();
    }
}