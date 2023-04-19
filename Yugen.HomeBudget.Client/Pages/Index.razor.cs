using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Models;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Client.Pages
{
    public partial class Index
    {
        private readonly int _year = DateTimeOffset.UtcNow.Year;
        private readonly int _month = DateTimeOffset.UtcNow.Month;

        private TotalExpense _currentYearTotalExpense;
        private TotalExpense _previousMonthTotalExpense;
        private TotalExpense _currentMonthTotalExpense;

        private PieChartData? pieChartData;

        private ICollection<ResponseExpenseDto>? _expenses;
        private PaginatedList<ResponseExpenseDto> _paginatedList = new PaginatedList<ResponseExpenseDto>();

        [Inject]
        private HttpClient _httpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetData();
        }

        private async Task GetData()
        {
            var data = await _httpClient.GetFromJsonAsync<List<ResponseExpenseGroupedByCategoryDto>>($"{EndpointConstants.GroupedByCategory}?year={_year}&month={_month}");
            if (data != null)
            {
                var labels = data.Select(x => x.Category).ToArray();
                var values = data.Select(x => x.Total).ToArray();
                var colors = data.Select(x => Models.Constants.ChartColors[x.Index]).ToArray();
                pieChartData = new PieChartData(labels, new PieChartDataset[]{ new PieChartDataset(colors, values) });
            }

            var currentYearTotalExpense = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.ExpenseSum}?year={_year}");
            var previousMonthExpenseTotal = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.ExpenseSum}?year={_year}&month={_month - 1}");
            var currentMonthExpenseTotal = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.ExpenseSum}?year={_year}&month={_month}");

            _currentYearTotalExpense = new TotalExpense(currentYearTotalExpense);
            _previousMonthTotalExpense = new TotalExpense(previousMonthExpenseTotal, previousMonthExpenseTotal);
            _currentMonthTotalExpense = new TotalExpense(currentMonthExpenseTotal, previousMonthExpenseTotal);

            _paginatedList = await _httpClient.GetFromJsonAsync<PaginatedList<ResponseExpenseDto>>($"{EndpointConstants.Expense}?year={_year}&month={_month}&pageNumber={1}&pageSize={Constants.PageSize}");
            _expenses = _paginatedList.Items;

            //catch (AccessTokenNotAvailableException exception)
            //exception.Redirect();
        }

        private async Task DeleteAsync(int id)
        {
            var result = await _httpClient.DeleteAsync($"{EndpointConstants.Expense}/{id}");
            //catch (AccessTokenNotAvailableException exception)
            //exception.Redirect();
            if (result.IsSuccessStatusCode)
            {
                var expense = _expenses?.FirstOrDefault(c => c.Id.Equals(id));
                if (expense != null)
                {
                    _expenses?.Remove(expense);
                }
            }
        }

        private string GetDate(DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString("d", DateTimeFormatInfo.CurrentInfo);
        }
    }
}