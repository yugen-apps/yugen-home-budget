using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Models;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Client.ViewModels.Expense;

internal sealed partial class ExpenseListViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private PaginatedList<ResponseExpenseDto> _paginatedList = new();

    [ObservableProperty]
    private int? _pageNumber = 1;

    [ObservableProperty]
    private int _year = DateTimeOffset.UtcNow.Year;

    [ObservableProperty]
    private int _month = DateTimeOffset.UtcNow.Month;

    public ExpenseListViewModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public ICollection<ResponseExpenseDto> Expenses => PaginatedList.Items;

    public static string GetDate(DateTimeOffset dateTimeOffset)
    {
        return dateTimeOffset.ToString("d", DateTimeFormatInfo.CurrentInfo);
    }

    public async Task LoadDataAsync()
    {
        IsLoading = true;

        try
        {
            var response = await _httpClient.GetFromJsonAsync<PaginatedList<ResponseExpenseDto>>($"{EndpointConstants.Expense}?year={Year}&month={Month}&pageNumber={PageNumber}&pageSize={Constants.PageSize}");
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

    public async Task PageIndexChanged(int newPageNumber)
    {
        if (newPageNumber < 1 ||
            newPageNumber > PaginatedList.TotalPages)
        {
            return;
        }

        PageNumber = newPageNumber;
        await LoadDataAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var result = await _httpClient.DeleteAsync($"{EndpointConstants.Expense}/{id}");
        if (result.IsSuccessStatusCode)
        {
            var category = Expenses?.FirstOrDefault(c => c.Id.Equals(id));
            if (category != null)
            {
                Expenses?.Remove(category);
            }
        }
    }

    public async Task DateChanged()
    {
        await LoadDataAsync();
    }
}