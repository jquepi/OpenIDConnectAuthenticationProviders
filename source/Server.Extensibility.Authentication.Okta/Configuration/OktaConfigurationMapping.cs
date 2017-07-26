using System;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;

namespace Octopus.Server.Extensibility.Authentication.Okta.Configuration
{
    public class OktaConfigurationMapping : IConfigurationDocumentMapper
    {
        public Type GetTypeToMap() => typeof(OktaConfiguration);
    }
}