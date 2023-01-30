using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Net.Http.Json;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Client.Pages.Expense
{
    public partial class List
    {
        [Inject]
        private HttpClient _httpClient { get; set; }

        private ICollection<ExpenseDto>? _expenses;

        protected override async Task OnInitializedAsync()
        {
            //try
            //{
                _expenses = await _httpClient.GetFromJsonAsync<ICollection<ExpenseDto>>("Expense");
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
                var result = await _httpClient.DeleteAsync($"Expense/{id}");
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