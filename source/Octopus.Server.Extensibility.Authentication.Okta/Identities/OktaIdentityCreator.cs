using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Identities;

namespace Octopus.Server.Extensibility.Authentication.Okta.Identities
{
    public class OktaIdentityCreator : IdentityCreator, IOktaIdentityCreator
    {
        protected override string ProviderName => OktaAuthenticationProvider.ProviderName;
    }

    public interface IOktaIdentityCreator : IIdentityCreator
    { }
}