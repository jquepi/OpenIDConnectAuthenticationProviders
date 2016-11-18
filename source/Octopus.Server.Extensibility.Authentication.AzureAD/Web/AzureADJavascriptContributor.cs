using System.Collections.Generic;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Content;

namespace Octopus.Server.Extensibility.Authentication.AzureAD.Web
{
    public class AzureADJavascriptContributor : IContributesJavascript, IContributesAngularModules
    {
        public IEnumerable<string> GetAngularModuleNames()
        {
            yield return "octopusApp.users.azureAD";
        }

        public IEnumerable<string> GetJavascriptUris(string requestDirectoryPath)
        {
            yield return "areas/users/azureAD_users_module.js";
            yield return "areas/users/controllers/azureAD_auth_provider_controller.js";
            yield return "areas/users/directives/azureAD_auth_provider.js";
        }
    }
}