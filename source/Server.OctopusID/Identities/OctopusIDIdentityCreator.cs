using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Identities;

namespace Octopus.Server.Extensibility.Authentication.OctopusID.Identities
{
    public class OctopusIDIdentityCreator : IdentityCreator, IOctopusIDIdentityCreator
    {
        protected override string ProviderName => OctopusIDAuthenticationProvider.ProviderName;
    }
}