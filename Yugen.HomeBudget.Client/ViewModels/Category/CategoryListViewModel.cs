using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Models;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Client.ViewModels.Category;

internal sealed partial class CategoryListViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private PaginatedList<ResponseCategoryDto> _paginatedList = new();

    [ObservableProperty]
    private int? _pageNumber = 1;

    public CategoryListViewModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public ICollection<ResponseCategoryDto> Categories => PaginatedList.Items;

    public async Task LoadDataAsync()
    {
        IsLoading = true;

        try
        {
            var response = await _httpClient.GetFromJsonAsync<PaginatedList<ResponseCategoryDto>>($"{EndpointConstants.Category}?pageNumber={PageNumber}&pageSize={Constants.PageSize}");
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
        var result = await _httpClient.DeleteAsync($"{EndpointConstants.Category}/{id}");
        if (result.IsSuccessStatusCode)
        {
            var category = Categories?.FirstOrDefault(c => c.Id.Equals(id));
            if (category != null)
            {
                Categories?.Remove(category);
            }
        }
    }
}