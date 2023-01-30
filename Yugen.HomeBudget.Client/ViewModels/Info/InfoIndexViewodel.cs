using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http.Json;
using Yugen.HomeBudget.Shared.Contants;

namespace Yugen.HomeBudget.Client.ViewModels.Info
{
    internal sealed partial class InfoIndexViewodel : ObservableObject
    {
        private readonly HttpClient _httpClient;

        [ObservableProperty]
        private Dictionary<string, string> _list = new();

        public InfoIndexViewodel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task LoadDataAsync()
        {
            List = await _httpClient.GetFromJsonAsync<Dictionary<string, string>>($"{EndpointConstants.Info}/get") ?? new();
        }
    }
}
