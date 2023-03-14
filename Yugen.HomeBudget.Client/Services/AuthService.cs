using System.Net.Http.Json;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<CurrentUser?> CurrentUserInfo() =>_httpClient.GetFromJsonAsync<CurrentUser>($"{EndpointConstants.Auth}/currentuserinfo");

    public async Task Login(LoginRequest loginRequest)
    {
        var result = await _httpClient.PostAsJsonAsync($"{EndpointConstants.Auth}/login", loginRequest);
        if (result.StatusCode != System.Net.HttpStatusCode.BadRequest)
        {
            result.EnsureSuccessStatusCode();
        }
        else
        {
            throw new Exception(await result.Content.ReadAsStringAsync());
        }
    }

    public async Task Logout()
    {
        var result = await _httpClient.PostAsync($"{EndpointConstants.Auth}/logout", null);
        result.EnsureSuccessStatusCode();
    }

    public async Task Register(RegisterRequest registerRequest)
    {
        var result = await _httpClient.PostAsJsonAsync($"{EndpointConstants.Auth}/register", registerRequest);
        if (result.StatusCode != System.Net.HttpStatusCode.BadRequest)
        {
            result.EnsureSuccessStatusCode();
        }
        else
        {
            throw new Exception(await result.Content.ReadAsStringAsync());
        }
    }
}