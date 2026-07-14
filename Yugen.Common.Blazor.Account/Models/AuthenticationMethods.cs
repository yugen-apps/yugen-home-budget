using MudBlazor;
using System.Collections.Generic;

namespace Yugen.Common.Blazor.Account.Models;

public static class AuthenticationMethods
{
    public static readonly AuthenticationMethod Google = new()
    {
        PictureClaimType = "google:picture",
        PicturePayloadKey = "picture",
        Icon = Icons.Custom.Brands.Google
    };

    public static AuthenticationMethod TryGet(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        return Dictionary.TryGetValue(name, out var authenticationMethod) ? authenticationMethod : null;
    }

    private static readonly Dictionary<string, AuthenticationMethod> Dictionary = new()
    {
           { nameof(Google), Google }
    };
}