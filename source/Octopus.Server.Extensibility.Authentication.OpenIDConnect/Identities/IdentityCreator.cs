using Octopus.Data.Model.User;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Identities
{
    public abstract class IdentityCreator : IIdentityCreator
    {
        public const string EmailClaimType = "email";
        public const string ExternalIdClaimType = "eid";

        protected abstract string ProviderName { get; }

        public Identity Create(string email, string externalId)
        {
            return new Identity(ProviderName)
                .WithClaim(EmailClaimType, email, true)
                .WithClaim(ExternalIdClaimType, externalId, true, true);
        }
    }
}