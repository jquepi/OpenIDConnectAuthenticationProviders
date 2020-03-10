using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Identities;

namespace Octopus.Server.Extensibility.Authentication.Okta.Identities
{
    class OktaIdentityCreator : IdentityCreator, IOktaIdentityCreator
    {
        protected override string ProviderName => OktaAuthenticationProvider.ProviderName;
    }

    interface IOktaIdentityCreator : IIdentityCreator
    { }
}