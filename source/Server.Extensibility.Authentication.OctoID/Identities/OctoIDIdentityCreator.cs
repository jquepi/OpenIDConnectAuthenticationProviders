using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Identities;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Identities
{
    public class OctoIDIdentityCreator : IdentityCreator, IOctoIDIdentityCreator
    {
        protected override string ProviderName => OctoIDAuthenticationProvider.ProviderName;
    }
}