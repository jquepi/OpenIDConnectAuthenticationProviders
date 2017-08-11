using Octopus.Server.Extensibility.Extensions.Infrastructure.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Configuration
{
    public interface IOpenIDConnectConfigurationStore : IExtensionConfigurationStore
    {
        string ConfigurationSettingsName { get; }

        string GetIssuer();
        void SetIssuer(string issuer);

        string GetResponseType();
        void SetResponseType(string responseType);

        string GetResponseMode();
        void SetResponseMode(string responseMode);

        string GetClientId();
        void SetClientId(string clientId);

        string GetScope();
        void SetScope(string scope);

        string GetNameClaimType();
        void SetNameClaimType(string nameClaimType);

        string GetLoginLinkLabel();
        void SetLoginLinkLabel(string loginLinkLabel);

        string RedirectUri { get; }

        bool GetAllowAutoUserCreation();
        void SetAllowAutoUserCreation(bool allowAutoUserCreation);
    }
}