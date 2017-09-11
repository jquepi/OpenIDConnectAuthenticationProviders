using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Identities;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Identities
{
    public class DataCenterManagerIdentityCreator : IdentityCreator, IDataCenterManagerIdentityCreator
    {
        protected override string ProviderName => DataCenterManagerAuthenticationProvider.ProviderName;
    }

    public interface IDataCenterManagerIdentityCreator : IIdentityCreator
    { }
}