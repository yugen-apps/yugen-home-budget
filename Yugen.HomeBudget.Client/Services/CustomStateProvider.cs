using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Client.Services;

public class CustomStateProvider : AuthenticationStateProvider
{
    private readonly IAuthService _api;
    private CurrentUser? _currentUser;

    public CustomStateProvider(IAuthService api)
    {
        _api = api;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();
        try
        {
            if (_currentUser == null ||
                _currentUser.IsAuthenticated == false)
            {
                _currentUser = await _api.CurrentUserInfo();
            }

            if (_currentUser?.IsAuthenticated ?? false)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, _currentUser.UserName) }.Concat(_currentUser.Claims.Select(c => new Claim(c.Key, c.Value)));
                identity = new ClaimsIdentity(claims, "Server authentication");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Request failed:" + ex);
        }

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task Logout()
    {
        await _api.Logout();
        _currentUser = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task Login(LoginRequest loginParameters)
    {
        await _api.Login(loginParameters);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task Register(RegisterRequest registerParameters)
    {
        await _api.Register(registerParameters);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<CurrentUser?> GetCurrentUser()
    {
        await GetAuthenticationStateAsync();
        
        return _currentUser;
    }
}