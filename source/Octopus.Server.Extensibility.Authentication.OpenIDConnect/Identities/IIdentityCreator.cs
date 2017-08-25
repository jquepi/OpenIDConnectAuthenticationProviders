using Octopus.Data.Model.User;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Identities
{
    public interface IIdentityCreator
    {
        Identity Create(string email, string externalId);
    }
}