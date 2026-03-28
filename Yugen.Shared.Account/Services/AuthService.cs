using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using Yugen.Shared.Account.Models;

namespace Yugen.Shared.Account.Services;

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