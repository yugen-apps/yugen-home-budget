using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Models;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Client.Pages.Category
{
    public partial class List
    {
        private ICollection<ResponseCategoryDto>? _categories;
        private PaginatedList<ResponseCategoryDto> _paginatedList = new PaginatedList<ResponseCategoryDto>();
        private int? _pageNumber = 1;

        [Inject]
        private HttpClient _httpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetData();
        }

        private async void PageIndexChanged(int newPageNumber)
        {
            if (newPageNumber < 1 ||
                newPageNumber > _paginatedList.TotalPages)
            {
                return;
            }

            _pageNumber = newPageNumber;
            await GetData();
            StateHasChanged();
        }

        private async Task GetData()
        {
            _paginatedList = await _httpClient.GetFromJsonAsync<PaginatedList<ResponseCategoryDto>>($"{EndpointConstants.Category}?pageNumber={_pageNumber}&pageSize={Constants.PageSize}");
            //catch (AccessTokenNotAvailableException exception)
            //exception.Redirect();
            _categories = _paginatedList.Items;
        }

        private async Task DeleteAsync(int id)
        {
            var result = await _httpClient.DeleteAsync($"{EndpointConstants.Category}/{id}");
            //catch (AccessTokenNotAvailableException exception)
            //exception.Redirect();
            if (result.IsSuccessStatusCode)
            {
                var category = _categories?.FirstOrDefault(c => c.Id.Equals(id));
                if (category != null)
                {
                    _categories?.Remove(category);
                }
            }
        }
    }
}