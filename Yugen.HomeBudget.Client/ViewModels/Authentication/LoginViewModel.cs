using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Client.Services;
using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Client.ViewModels.Authentication;

internal sealed partial class LoginViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly CustomStateProvider _authStateProvider;

    [ObservableProperty]
    private LoginRequest _loginRequest = new LoginRequest();

    [ObservableProperty]
    private string _error;

    public LoginViewModel(
                NavigationManager navigationManager,
        CustomStateProvider authStateProvider)
    {
        _navigationManager = navigationManager;
        _authStateProvider = authStateProvider;
    }

    public async Task OnSubmit()
    {
        Error = null;
        try
        {
            await _authStateProvider.Login(LoginRequest);
            _navigationManager.NavigateTo("");
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
    }
}