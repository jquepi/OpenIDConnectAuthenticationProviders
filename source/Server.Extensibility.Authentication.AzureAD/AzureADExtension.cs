using Autofac;
using Octopus.Node.Extensibility.Authentication.Extensions;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Certificates;
using Octopus.Node.Extensibility.Extensions;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Web.Content;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Server.Extensibility.Authentication.AzureAD.Infrastructure;
using Octopus.Server.Extensibility.Authentication.AzureAD.Issuer;
using Octopus.Server.Extensibility.Authentication.AzureAD.Tokens;
using Octopus.Server.Extensibility.Authentication.AzureAD.Web;

namespace Octopus.Server.Extensibility.Authentication.AzureAD
{
    [OctopusPlugin("AzureAD", "Octopus Deploy")]
    public class AzureADExtension : OpenIDConnectExtension, IOctopusExtension
    {
        public override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AzureADPrincipalToUserResourceMapper>().As<IAzureADPrincipalToUserResourceMapper>().InstancePerDependency();
            builder.RegisterType<AzureADConfigurationMapping>().As<IConfigurationDocumentMapper>().InstancePerDependency();

            builder.RegisterType<AzureADConfigurationStore>()
                .As<IAzureADConfigurationStore>()
                .As<IHasConfigurationSettings>()
                .InstancePerDependency();
            builder.RegisterType<AzureADConfigureCommands>()
                .As<IContributeToConfigureCommand>()
                .As<IHandleLegacyWebAuthenticationModeConfigurationCommand>()
                .InstancePerDependency();

            builder.RegisterType<AzureADAuthorizationEndpointUrlBuilder>().As<IAzureADAuthorizationEndpointUrlBuilder>().InstancePerDependency();
            builder.RegisterType<AzureADAuthTokenHandler>().As<IAzureADAuthTokenHandler>().InstancePerDependency();

            builder.RegisterType<AzureADHomeLinksContributor>().As<IHomeLinksContributor>().InstancePerDependency();

            // These are important as Singletons because they cache X509 certificates for performance
            builder.RegisterType<DefaultCertificateJsonParser>().As<ICertificateJsonParser>().SingleInstance();
            builder.RegisterType<AzureADCertificateRetriever>().As<ICertificateRetriever>().SingleInstance();

            builder.RegisterType<AzureADStaticContentFolders>().As<IContributesStaticContentFolders>().InstancePerDependency();

            builder.RegisterType<AzureADUserAuthenticationAction>().AsSelf().InstancePerDependency();
            builder.RegisterType<AzureADUserAuthenticatedAction>().AsSelf().InstancePerDependency();

            builder.RegisterType<AzureADCSSContributor>().As<IContributesCSS>().InstancePerDependency();
            builder.RegisterType<AzureADJavascriptContributor>()
                .As<IContributesJavascript>()
                .As<IContributesAngularModules>()
                .InstancePerDependency();

            builder.RegisterType<AzureADAuthenticationProvider>()
                .As<IAuthenticationProvider>()
                .As<Octopus.Node.Extensibility.Authentication.Extensions.IAuthenticationProviderWithGroupSupport>()
                .AsSelf()
                .InstancePerDependency();
        }
    }
}