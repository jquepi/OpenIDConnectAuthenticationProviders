using System.Collections.Generic;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Content;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps.Web
{
    public class GoogleAppsJavascriptContributor : IContributesJavascript, IContributesAngularModules
    {
        public IEnumerable<string> GetAngularModuleNames()
        {
            yield return "octopusApp.users.google";
        }

        public IEnumerable<string> GetJavascriptUris(string requestDirectoryPath)
        {
            yield return "areas/users/googleApps_users_module.js";
            yield return "areas/users/controllers/googleApps_auth_provider_controller.js";
            yield return "areas/users/directives/googleApps_auth_provider.js";
        }
    }
}