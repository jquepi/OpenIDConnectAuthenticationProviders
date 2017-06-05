using System.Linq;
using System.Security.Claims;
using Octopus.Diagnostics;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Issuer;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Tokens;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Issuer;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens
{
    public class GoogleAuthTokenHandler : OpenIDConnectAuthTokenHandler<IGoogleAppsConfigurationStore, IGoogleCertificateRetriever>, IGoogleAuthTokenHandler
    {
        public GoogleAuthTokenHandler(ILog log, IGoogleAppsConfigurationStore configurationStore, IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, IGoogleCertificateRetriever certificateRetriever) : base(log, configurationStore, identityProviderConfigDiscoverer, certificateRetriever)
        {
        }

        protected override void DoIssuerSpecificClaimsValidation(ClaimsPrincipal principal, out string error)
        {
            error = string.Empty;
            var hd = ConfigurationStore.GetHostedDomain();
            var hdClaim = principal.Claims.FirstOrDefault(c => c.Type == "hd");
            if (hdClaim == null || hdClaim.Value != hd)
            {
                error = "Incorrect Hosted Domain value. This server is setup to accept users from a specific hosted domain only.";
            }
        }
    }
}