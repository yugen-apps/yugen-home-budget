using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Client.Pages.Category
{
    public partial class List
    {
        [Inject]
        private HttpClient _httpClient { get; set; }
        
        private ICollection<CategoryDto>? _categories;
        
        protected override async Task OnInitializedAsync()
        {
            //try
            //{
                _categories = await _httpClient.GetFromJsonAsync<ICollection<CategoryDto>>("Category");
            //}
            //catch (AccessTokenNotAvailableException exception)
            //{
                //exception.Redirect();
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