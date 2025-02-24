using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Client.Services;
using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Client.ViewModels.Authentication;

public sealed partial class RegisterViewModel : ObservableObject
{
    public bool IsAlertVisible;
    public Validations validations = new();
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
        if (await validations.ValidateAll())
        {
            try
            {
                await validations.ClearAll();
                await _authStateProvider.Register(RegisterRequest);
                _navigationManager.NavigateTo("");
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
        IsAlertVisible = !string.IsNullOrWhiteSpace(Error);
    }
}