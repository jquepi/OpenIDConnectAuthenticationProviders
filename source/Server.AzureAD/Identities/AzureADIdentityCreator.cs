using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Identities;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Identities
{
    public class AzureADIdentityCreator : IdentityCreator, IAzureADIdentityCreator
    {
        protected override string ProviderName => AzureADAuthenticationProvider.ProviderName;
    }

    public interface IAzureADIdentityCreator : IIdentityCreator
    { }
}