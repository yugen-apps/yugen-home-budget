using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Models;
using Yugen.HomeBudget.Shared.Contants;

namespace Yugen.HomeBudget.Client.Pages
{
    public partial class Index
    {
        private TotalExpense _previousMonthTotalExpense;
        private TotalExpense _currentMonthTotalExpense;
        private readonly int _year = DateTimeOffset.UtcNow.Year;
        private readonly int _month = DateTimeOffset.UtcNow.Month;

        [Inject]
        private HttpClient _httpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetData();
        }

        private async Task GetData()
        {
            var previousMonthExpenseTotal = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.Expense}/sum?year={_year}&month={_month-1}");
            var currentMonthExpenseTotal = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.Expense}/sum?year={_year}&month={_month}");

            _previousMonthTotalExpense = new TotalExpense(previousMonthExpenseTotal, previousMonthExpenseTotal);
            _currentMonthTotalExpense = new TotalExpense(currentMonthExpenseTotal, previousMonthExpenseTotal);

            //catch (AccessTokenNotAvailableException exception)
            //exception.Redirect();
        }
    }
}