using System.Linq;
using System.Security.Claims;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Issuer;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Tokens;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens
{
    public class GoogleAuthTokenHandler : OpenIDConnectAuthTokenHandler<IGoogleAppsConfigurationStore, IGoogleCertificateRetriever>, IGoogleAuthTokenHandler
    {
        public GoogleAuthTokenHandler(IGoogleAppsConfigurationStore configurationStore, IIdentityProviderConfigDiscoverer identityProviderConfigDiscoverer, IGoogleCertificateRetriever certificateRetriever) : base(configurationStore, identityProviderConfigDiscoverer, certificateRetriever)
        {
        }

        protected override void DoIssuerSpecificClaimsValidation(ClaimsPrincipal principal)
        {
            var hd = ConfigurationStore.GetHostedDomain();

            var hdClaim = principal.Claims.FirstOrDefault(c => c.Type == "hd");

            if (hdClaim == null || hdClaim.Value != hd)
            {
                throw new InvalidHostedDomainSecurityException();
            }
        }
    }
}