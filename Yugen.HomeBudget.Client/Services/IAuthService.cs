using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Client.Services;

public interface IAuthService
{
    Task Login(LoginRequest loginRequest);

    Task Register(RegisterRequest registerRequest);

    Task Logout();

    Task<CurrentUser> CurrentUserInfo();
}