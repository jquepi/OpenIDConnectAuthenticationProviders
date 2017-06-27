using System;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration
{
    public class DataCenterManagerConfigurationMapping : IConfigurationDocumentMapper
    {
        public Type GetTypeToMap() => typeof(DataCenterManagerConfiguration);
    }
}