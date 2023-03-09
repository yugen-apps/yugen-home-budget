using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Models;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Client.Pages.Expense
{
    public partial class List
    {
        private ICollection<ExpenseDto>? _expenses;

        private PaginatedList<ExpenseDto> _paginatedList = new PaginatedList<ExpenseDto>();

        private int? _pageNumber = 1;

        [Inject]
        private HttpClient _httpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetData();
        }

        private async void PageIndexChanged(int newPageNumber)
        {
            if (newPageNumber < 1 || newPageNumber > _paginatedList.TotalPages)
            {
                return;
            }

            _pageNumber = newPageNumber;
            await GetData();
            StateHasChanged();
        }

        private async Task GetData()
        {
            //try
            //{
            _paginatedList = await _httpClient.GetFromJsonAsync<PaginatedList<ExpenseDto>>($"{EndpointConstants.Expense}?pageNumber={_pageNumber}&pageSize={Constants.PageSize}");
            _expenses = _paginatedList.Items;
            //}
            //catch (AccessTokenNotAvailableException exception)
            //{
            //    exception.Redirect();
            //}
        }

        private async Task DeleteAsync(int id)
        {
            //try
            //{
            var result = await _httpClient.DeleteAsync($"{EndpointConstants.Expense}/{id}");
            if (result.IsSuccessStatusCode)
            {
                var expense = _expenses?.FirstOrDefault(c => c.Id.Equals(id));
                if (expense != null)
                {
                    _expenses?.Remove(expense);
                }
            }
            //}
            //catch (AccessTokenNotAvailableException exception)
            //{
            //    exception.Redirect();
            //}
        }

        private string GetDate(DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString("d", DateTimeFormatInfo.CurrentInfo);
        }
    }
}