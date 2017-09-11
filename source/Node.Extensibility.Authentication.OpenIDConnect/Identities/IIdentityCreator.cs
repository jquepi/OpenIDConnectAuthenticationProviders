using Octopus.Data.Model.User;

namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Identities
{
    public interface IIdentityCreator
    {
        Identity Create(string email, string displayName, string externalId);
    }
}