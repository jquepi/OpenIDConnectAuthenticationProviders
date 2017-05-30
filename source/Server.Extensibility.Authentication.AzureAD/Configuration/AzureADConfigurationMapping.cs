using System;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Configuration
{
    public class AzureADConfigurationMapping : IConfigurationDocumentMapper
    {
        public Type GetTypeToMap() => typeof(AzureADConfiguration);
    }
}