using System;
using System.Linq;
using Octopus.Server.Extensibility.Authentication.Extensions;
using Octopus.Server.Extensibility.Authentication.Okta.Configuration;
using Octopus.Server.Extensibility.Authentication.Web;

namespace Octopus.Server.Extensibility.Authentication.Okta.Web
{
    class OktaLoginParametersHandler : ICanHandleLoginParameters
    {
        readonly IOktaConfigurationStore configurationStore;

        public OktaLoginParametersHandler(IOktaConfigurationStore configurationStore)
        {
            this.configurationStore = configurationStore;
        }

        public LoginInitiatedResult? WasExternalLoginInitiated(string encodedQueryString)
        {
            if (!configurationStore.GetIsEnabled())
                return null;

            var parser = new EncodedQueryStringParser();
            var parameters = parser.Parse(encodedQueryString);

            var issuerParam = parameters.FirstOrDefault(p => p.Name == "iss");

            var configuredIssuer = configurationStore.GetIssuer();

            if (issuerParam != null && string.Compare(configuredIssuer, issuerParam.Value, StringComparison.InvariantCultureIgnoreCase) == 0)
                return new LoginInitiatedResult(OktaAuthenticationProvider.ProviderName);

            return null;
        }
    }
}