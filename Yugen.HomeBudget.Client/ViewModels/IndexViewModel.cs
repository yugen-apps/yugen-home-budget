using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Models;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Client.ViewModels;

internal sealed partial class IndexViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private ICollection<ResponseExpenseDto>? _expenses;

    [ObservableProperty]
    private PaginatedList<ResponseExpenseDto> _paginatedList = new PaginatedList<ResponseExpenseDto>();

    [ObservableProperty]
    private int? _pageNumber = 1;

    [ObservableProperty]
    private int _year = DateTimeOffset.UtcNow.Year;

    [ObservableProperty]
    private int _month = DateTimeOffset.UtcNow.Month;

    [ObservableProperty]
    private TotalExpense _currentYearTotalExpense;

    [ObservableProperty]
    private TotalExpense _previousMonthTotalExpense;

    [ObservableProperty]
    private TotalExpense _currentMonthTotalExpense;

    [ObservableProperty]
    private PieChartData? _pieChartData;

    public IndexViewModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task LoadDataAsync()
    {
        IsLoading = true;

        try
        {
            var data = await _httpClient.GetFromJsonAsync<List<ResponseExpenseGroupedByCategoryDto>>($"{EndpointConstants.GroupedByCategory}?year={_year}&month={_month}");
            if (data != null)
            {
                var labels = data.Select(x => x.Category).ToArray();
                var values = data.Select(x => x.Total).ToArray();
                var colors = data.Select(x => Models.Constants.ChartColors[x.Index]).ToArray();
                PieChartData = new PieChartData(labels, new PieChartDataset[] { new PieChartDataset(colors, values) });
            }

            var currentYearTotalExpense = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.ExpenseSum}?year={_year}");
            var previousMonthExpenseTotal = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.ExpenseSum}?year={_year}&month={_month - 1}");
            var currentMonthExpenseTotal = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.ExpenseSum}?year={_year}&month={_month}");

            CurrentYearTotalExpense = new TotalExpense(currentYearTotalExpense);
            PreviousMonthTotalExpense = new TotalExpense(previousMonthExpenseTotal, previousMonthExpenseTotal);
            CurrentMonthTotalExpense = new TotalExpense(currentMonthExpenseTotal, previousMonthExpenseTotal);

            PaginatedList = await _httpClient.GetFromJsonAsync<PaginatedList<ResponseExpenseDto>>($"{EndpointConstants.Expense}?year={_year}&month={_month}&pageNumber={_pageNumber}&pageSize={Constants.PageSize}");
            Expenses = PaginatedList.Items;

            //catch (AccessTokenNotAvailableException exception)
            //exception.Redirect();
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

    public string GetDate(DateTimeOffset dateTimeOffset)
    {
        return dateTimeOffset.ToString("d", DateTimeFormatInfo.CurrentInfo);
    }
}