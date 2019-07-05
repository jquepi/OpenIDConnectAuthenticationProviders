using System;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctoID.Configuration
{
    public class OctoIDConfigurationMapping : IConfigurationDocumentMapper
    {
        public Type GetTypeToMap() => typeof(OctoIDConfiguration);
    }
}