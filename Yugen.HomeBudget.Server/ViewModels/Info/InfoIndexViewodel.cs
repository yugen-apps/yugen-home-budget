using CommunityToolkit.Mvvm.ComponentModel;
using Yugen.HomeBudget.Server.Services;

namespace Yugen.HomeBudget.Server.ViewModels.Info
{
    public sealed partial class InfoIndexViewodel : ObservableObject
    {
        private readonly InfoService _infoService;

        [ObservableProperty]
        private Dictionary<string, string> _list = new();

        public InfoIndexViewodel(InfoService infoService)
        {
            _infoService = infoService;
        }

        public async Task LoadDataAsync()
        {
            List = await _infoService.GetAsync();
        }
    }
}
