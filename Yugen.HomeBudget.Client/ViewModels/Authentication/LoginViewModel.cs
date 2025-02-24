using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Client.Services;
using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Client.ViewModels.Authentication;

public sealed partial class LoginViewModel : ObservableObject
{
    public bool IsAlertVisible;
    public Validations validations = new();
    private readonly CustomStateProvider _authStateProvider;
    private readonly NavigationManager _navigationManager;

    [ObservableProperty]
    private string? _error;

    [ObservableProperty]
    private LoginRequest _loginRequest = new();

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
        if (await validations.ValidateAll())
        {
            try
            {
                await validations.ClearAll();
                await _authStateProvider.Login(LoginRequest);
                _navigationManager.NavigateTo("", true);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
        IsAlertVisible = !string.IsNullOrWhiteSpace(Error);
    }
}