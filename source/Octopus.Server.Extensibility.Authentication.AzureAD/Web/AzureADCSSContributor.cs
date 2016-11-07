using System.Collections.Generic;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Content;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADCSSContributor : IContributesCSS
    {
        public IEnumerable<string> GetCSSUris(string requestDirectoryPath)
        {
            yield return $"{requestDirectoryPath}/styles/azureAD.css";
        }
    }
}