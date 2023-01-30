using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Client.Services;
using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Client.ViewModels.Authentication;

internal sealed partial class RegisterViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly CustomStateProvider _authStateProvider;

    [ObservableProperty]
    private RegisterRequest _registerRequest = new();

    [ObservableProperty]
    private string? _error;

    public RegisterViewModel(
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
            await _authStateProvider.Register(RegisterRequest);
            _navigationManager.NavigateTo("");
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
    }
}