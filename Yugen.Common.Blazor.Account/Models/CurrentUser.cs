using System.Security.Claims;

namespace Yugen.Common.Blazor.Account.Models;

public class CurrentUser
{
    public CurrentUser(ClaimsPrincipal user)
    {
        User = user;
        IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;

        if (IsAuthenticated)
        {
            UserName = User?.Identity?.Name;
            GivenName = User?.FindFirst(ClaimTypes.GivenName)?.Value;
            Surname = User?.FindFirst(ClaimTypes.Surname)?.Value;

            var currentUserIdentifier = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(currentUserIdentifier, out var currentUserId))
            {
                Id = currentUserId;
            }

            var authenticationMethodName = User?.FindFirst(ClaimTypes.AuthenticationMethod)?.Value;
            AuthenticationMethod = AuthenticationMethods.TryGet(authenticationMethodName);
            if (AuthenticationMethod == null)
            {
                return;
            }

            Avatar = User?.FindFirst(AuthenticationMethod?.PictureClaimType)?.Value;
        }
    }

    public ClaimsPrincipal User { get; }

    public bool IsAuthenticated { get; }

    public string UserName { get; }

    public string GivenName { get; }

    public string Surname { get; }

    public AuthenticationMethod AuthenticationMethod { get; private set; }

    public string Avatar { get; }

    public int Id { get; set; }
}
