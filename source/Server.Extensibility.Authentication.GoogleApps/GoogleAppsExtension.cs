using Autofac;
using Octopus.Node.Extensibility.Authentication.Extensions;
using Octopus.Node.Extensibility.Authentication.Extensions.Identities;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Node.Extensibility.Extensions;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Web.Content;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Identities;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Issuer;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Web;
using Octopus.Server.Extensibility.HostServices.Web;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps
{
    [OctopusPlugin("GoogleApps", "Octopus Deploy")]
    public class GoogleAppsExtension : OpenIDConnectExtension, IOctopusExtension
    {
        public override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<GoogleAppsConfigurationMapping>().As<IConfigurationDocumentMapper>().InstancePerDependency();

            builder.RegisterType<GoogleAppsIdentityCreator>().As<IGoogleAppsIdentityCreator>().SingleInstance();

            builder.RegisterType<GoogleAppsConfigurationStore>()
                .As<IGoogleAppsConfigurationStore>()
                .As<IHasConfigurationSettings>()
                .InstancePerDependency();
            builder.RegisterType<GoogleAppsConfigureCommands>()
                .As<IContributeToConfigureCommand>()
                .InstancePerDependency();

            builder.RegisterType<UrlEncoder>().As<IUrlEncoder>().InstancePerDependency();

            builder.RegisterType<GoogleAppsAuthorizationEndpointUrlBuilder>().As<IGoogleAppsAuthorizationEndpointUrlBuilder>().InstancePerDependency();
            builder.RegisterType<GoogleAuthTokenHandler>().As<IGoogleAuthTokenHandler>().InstancePerDependency();

            builder.RegisterType<GoogleAppsHomeLinksContributor>().As<IHomeLinksContributor>().InstancePerDependency();
             
            builder.RegisterType<GoogleAppsStaticContentFolders>().As<IContributesStaticContentFolders>().InstancePerDependency();

            // These are important as Singletons because they cache X509 certificates for performance
            builder.RegisterType<DefaultKeyJsonParser>().As<IKeyJsonParser>().SingleInstance();
            builder.RegisterType<GoogleKeyRetriever>().As<IGoogleKeyRetriever>().SingleInstance();

            builder.RegisterType<GoogleAppsUserAuthenticationAction>().AsSelf().InstancePerDependency();
            builder.RegisterType<GoogleAppsUserAuthenticatedAction>().AsSelf().InstancePerDependency();

            builder.RegisterType<GoogleAppsAuthenticationProvider>()
                .As<IAuthenticationProvider>()
                .As<IAuthenticationProviderWithGroupSupport>()
                .As<IContributesCSS>()
                .As<IContributesJavascript>()
                .As<IUseAuthenticationIdentities>()
                .AsSelf()
                .InstancePerDependency();
        }
    }
}