using Microsoft.AspNetCore.Components.Authorization;
using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Server.Services;

public class AuthService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthService(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<CurrentUser> GetCurrentUserAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

        var user = authState.User;
        return new CurrentUser(user);
    }
}