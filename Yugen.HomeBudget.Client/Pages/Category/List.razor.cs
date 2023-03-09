using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Models;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Client.Pages.Category
{
    public partial class List
    {
        private ICollection<CategoryDto>? _categories;

        private PaginatedList<CategoryDto> _paginatedList = new PaginatedList<CategoryDto>();

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
            _paginatedList = await _httpClient.GetFromJsonAsync<PaginatedList<CategoryDto>>($"Category?pageNumber={_pageNumber}&pageSize={Constants.PageSize}");
            _categories = _paginatedList.Items;
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
            var result = await _httpClient.DeleteAsync($"Category/{id}");
            if (result.IsSuccessStatusCode)
            {
                var category = _categories?.FirstOrDefault(c => c.Id.Equals(id));
                if (category != null)
                {
                    _categories?.Remove(category);
                }
            }
            //}
            //catch (AccessTokenNotAvailableException exception)
            //{
            //    exception.Redirect();
            //}
        }
    }
}