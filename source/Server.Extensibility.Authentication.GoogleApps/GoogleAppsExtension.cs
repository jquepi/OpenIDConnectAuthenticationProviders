using Autofac;
using Octopus.Node.Extensibility.Authentication.Extensions;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect;
using Octopus.Node.Extensibility.Extensions;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Web.Content;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Configuration;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Issuer;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Tokens;
using Octopus.Server.Extensibility.Authentication.GoogleApps.Web;

namespace Octopus.Server.Extensibility.Authentication.GoogleApps
{
    [OctopusPlugin("GoogleApps", "Octopus Deploy")]
    public class GoogleAppsExtension : OpenIDConnectExtension, IOctopusExtension
    {
        public override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<GoogleAppsConfigurationMapping>().As<IConfigurationDocumentMapper>().InstancePerDependency();

            builder.RegisterType<GoogleAppsConfigurationStore>()
                .As<IGoogleAppsConfigurationStore>()
                .As<IHasConfigurationSettings>()
                .InstancePerDependency();
            builder.RegisterType<GoogleAppsConfigureCommands>()
                .As<IContributeToConfigureCommand>()
                .As<IHandleLegacyWebAuthenticationModeConfigurationCommand>()
                .InstancePerDependency();

            builder.RegisterType<GoogleAppsAuthorizationEndpointUrlBuilder>().As<IGoogleAppsAuthorizationEndpointUrlBuilder>().InstancePerDependency();
            builder.RegisterType<GoogleAuthTokenHandler>().As<IGoogleAuthTokenHandler>().InstancePerDependency();

            builder.RegisterType<GoogleAppsHomeLinksContributor>().As<IHomeLinksContributor>().InstancePerDependency();
             
            builder.RegisterType<GoogleAppsStaticContentFolders>().As<IContributesStaticContentFolders>().InstancePerDependency();

            // These are important as Singletons because they cache X509 certificates for performance
            builder.RegisterType<GoogleCertificateJsonParser>().As<IGoogleCertificateJsonParser>().SingleInstance();
            builder.RegisterType<GoogleCertificateRetriever>().As<IGoogleCertificateRetriever>().SingleInstance();

            builder.RegisterType<GoogleAppsUserAuthenticationAction>().AsSelf().InstancePerDependency();
            builder.RegisterType<GoogleAppsUserAuthenticatedAction>().AsSelf().InstancePerDependency();

            builder.RegisterType<GoogleAppsCSSContributor>().As<IContributesCSS>().InstancePerDependency();
            builder.RegisterType<GoogleAppsJavascriptContributor>()
                .As<IContributesJavascript>()
                .As<IContributesAngularModules>()
                .InstancePerDependency();

            builder.RegisterType<GoogleAppsAuthenticationProvider>()
                .As<IAuthenticationProvider>()
                .As<Octopus.Node.Extensibility.Authentication.Extensions.IAuthenticationProviderWithGroupSupport>()
                .AsSelf()
                .InstancePerDependency();
        }
    }
}