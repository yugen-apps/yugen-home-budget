using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yugen.HomeBudget.Server.Services;

namespace Yugen.HomeBudget.Server.ViewModels.Info
{
    public sealed partial class InfoIndexViewodel : ObservableObject
    {
        private readonly InfoService _infoService;

        [ObservableProperty]
        private Dictionary<string, string> _list = [];

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
