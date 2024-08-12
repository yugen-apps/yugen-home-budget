using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;

namespace Yugen.HomeBudget.Server.AuthenticationProviders
{
    public class AzureCredentialSqlAuthenticationProvider : SqlAuthenticationProvider
    {
        private readonly bool _isDevelopment;

        public AzureCredentialSqlAuthenticationProvider(bool isDevelopment)
        {
            _isDevelopment = isDevelopment;
        }

        public async override Task<SqlAuthenticationToken> AcquireTokenAsync(SqlAuthenticationParameters parameters)
        {
            // https://github.com/Azure/azure-sdk-for-net/issues/20210
            var azureTokenCredential = new ChainedTokenCredential(new ManagedIdentityCredential());

            if (_isDevelopment)
            {
                azureTokenCredential = new ChainedTokenCredential(
                    new AzureCliCredential(),
                    new VisualStudioCodeCredential(),
                    new VisualStudioCredential()
                // new InteractiveBrowserCredential()
                // Do Not use DefaultAzureCredential
                // new DefaultAzureCredential(includeInteractiveCredentials: true),
                );
            }

            var tokenResponse = await azureTokenCredential.GetTokenAsync(new TokenRequestContext([parameters.Resource]), cancellationToken: default);

            return new SqlAuthenticationToken(tokenResponse.Token, tokenResponse.ExpiresOn);
        }

        public override bool IsSupported(SqlAuthenticationMethod authenticationMethod)
        {
            return authenticationMethod == SqlAuthenticationMethod.ActiveDirectoryManagedIdentity
                || authenticationMethod == SqlAuthenticationMethod.ActiveDirectoryMSI;
        }
    }
}