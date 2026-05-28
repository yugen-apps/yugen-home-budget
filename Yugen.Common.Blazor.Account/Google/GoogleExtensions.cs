using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Models;
using Yugen.Home.Budget.Data.Models;

namespace Yugen.Common.Blazor.Account.Extensions;

public static class GoogleExtensions
{
	public static AuthenticationBuilder TryAddGoogleAuth(
		this AuthenticationBuilder authenticationBuilder,
		ConfigurationManager configuration,
		string configSectionName)
	{
		var googleAuthConfig = configuration.GetSection(configSectionName).Get<GoogleConfig>();
		if (googleAuthConfig == null)
		{
			return authenticationBuilder;
		}

		authenticationBuilder.AddGoogle(options =>
		 {
			 options.ClientId = googleAuthConfig.ClientId;
			 options.ClientSecret = googleAuthConfig.ClientSecret;
			 options.ClaimActions.MapJsonKey(AuthenticationMethods.Google.PictureClaimType, AuthenticationMethods.Google.PicturePayloadKey);
		 });

		return authenticationBuilder;
	}

	public static async Task<bool> TryAddClaims(this UserManager<ApplicationUser> userManager, ApplicationUser user, ExternalLoginInfo externalLoginInfo)
	{
		if (externalLoginInfo == null)
		{
			return false;
		}

		IEnumerable<string> types = [
			ClaimTypes.GivenName,
			ClaimTypes.Surname,
			AuthenticationMethods.Google.PictureClaimType
		];

		var userClaims = await userManager.GetClaimsAsync(user);
		bool refreshSignIn = false;
		foreach (var type in types)
		{
			var externalClaim = externalLoginInfo.Principal.FindFirst(type);
			if (externalClaim == null)
			{
				continue;
			}

			var userClaim = userClaims.FirstOrDefault(c => c.Type == type);
			if (userClaim == null)
			{
				await userManager.AddClaimAsync(user, externalClaim);
				refreshSignIn = true;
			}
			else if (userClaim.Value != externalClaim.Value)
			{
				await userManager.ReplaceClaimAsync(user, userClaim, externalClaim);
				refreshSignIn = true;
			}
		}
		return refreshSignIn;
	}
}
