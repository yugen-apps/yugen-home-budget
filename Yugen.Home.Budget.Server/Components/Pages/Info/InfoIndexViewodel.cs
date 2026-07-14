using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Services;
using Yugen.Common.Blazor.Services.Info;

namespace Yugen.Home.Budget.Server.ViewModels.Info;

public sealed partial class InfoIndexViewodel : ObservableObject
{
    private readonly IInfoService _infoService;
    private readonly AuthService _authService;

    [ObservableProperty]
    public partial Dictionary<string, string> List { get; set; } = [];

    public InfoIndexViewodel(
        IInfoService infoService,
        AuthService authService)
    {
        _infoService = infoService;
        _authService = authService;
    }

    public async Task LoadDataAsync()
    {
        var currentUser = await _authService.GetCurrentUserAsync();

        List = await _infoService.GetAsync(currentUser?.IsAuthenticated ?? false);
    }
}
