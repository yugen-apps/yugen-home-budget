using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Yugen.HomeBudget.Shared.Contants;

namespace Yugen.HomeBudget.Client.Pages
{
    public partial class Index
    {
        private decimal _total;
        private int _year = DateTimeOffset.UtcNow.Year;
        private int _month = DateTimeOffset.UtcNow.Month;

        [Inject]
        private HttpClient _httpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetData();
        }

        private async Task GetData()
        {
            _total = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.Expense}/sum?year={_year}&month={_month}");
            //catch (AccessTokenNotAvailableException exception)
            //exception.Redirect();
        }
    }
}