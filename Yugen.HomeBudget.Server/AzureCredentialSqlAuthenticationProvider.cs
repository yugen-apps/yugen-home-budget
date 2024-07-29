using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;

public class AzureCredentialSqlAuthenticationProvider : SqlAuthenticationProvider
{
	public async override Task<SqlAuthenticationToken> AcquireTokenAsync(SqlAuthenticationParameters parameters)
	{
		var azureTokenCredential = new ChainedTokenCredential(
			new ManagedIdentityCredential(parameters.UserId == string.Empty ? null : parameters.UserId),
			new AzureCliCredential(),
			new VisualStudioCodeCredential(),
			new VisualStudioCredential());

		var tokenResponse = await azureTokenCredential.GetTokenAsync(new TokenRequestContext(new[] { parameters.Resource }), cancellationToken: default);

		return new SqlAuthenticationToken(tokenResponse.Token, tokenResponse.ExpiresOn);
	}

	public override bool IsSupported(SqlAuthenticationMethod authenticationMethod)
	{
		return authenticationMethod == SqlAuthenticationMethod.ActiveDirectoryManagedIdentity
			|| authenticationMethod == SqlAuthenticationMethod.ActiveDirectoryMSI;
	}
}