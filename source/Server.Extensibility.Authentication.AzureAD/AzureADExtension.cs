using Autofac;
using Octopus.Node.Extensibility.Authentication.Extensions;
using Octopus.Node.Extensibility.Authentication.Extensions.Identities;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Node.Extensibility.Extensions;
using Octopus.Node.Extensibility.Extensions.Infrastructure;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Web.Content;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Server.Extensibility.Authentication.AzureAD.Configuration;
using Octopus.Server.Extensibility.Authentication.AzureAD.Identities;
using Octopus.Server.Extensibility.Authentication.AzureAD.Infrastructure;
using Octopus.Server.Extensibility.Authentication.AzureAD.Issuer;
using Octopus.Server.Extensibility.Authentication.AzureAD.Tokens;
using Octopus.Server.Extensibility.Authentication.AzureAD.Web;
using Octopus.Server.Extensibility.HostServices.Web;

namespace Octopus.Server.Extensibility.Authentication.AzureAD
{
    [OctopusPlugin("AzureAD", "Octopus Deploy")]
    public class AzureADExtension : OpenIDConnectExtension, IOctopusExtension
    {
        public override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AzureADDatabaseInitializer>().As<IExecuteWhenDatabaseInitializes>().InstancePerDependency();
            builder.RegisterType<AzureADPrincipalToUserResourceMapper>().As<IAzureADPrincipalToUserResourceMapper>().InstancePerDependency();
            builder.RegisterType<AzureADConfigurationMapping>().As<IConfigurationDocumentMapper>().InstancePerDependency();

            builder.RegisterType<AzureADIdentityCreator>().As<IAzureADIdentityCreator>().SingleInstance();

            builder.RegisterType<AzureADConfigurationStore>()
                .As<IAzureADConfigurationStore>()
                .As<IHasConfigurationSettings>()
                .InstancePerDependency();
            builder.RegisterType<AzureADConfigureCommands>()
                .As<IContributeToConfigureCommand>()
                .InstancePerDependency();

            builder.RegisterType<UrlEncoder>().As<IUrlEncoder>().InstancePerDependency();
            
            builder.RegisterType<AzureADAuthorizationEndpointUrlBuilder>().As<IAzureADAuthorizationEndpointUrlBuilder>().InstancePerDependency();
            builder.RegisterType<AzureADAuthTokenHandler>().As<IAzureADAuthTokenHandler>().InstancePerDependency();

            builder.RegisterType<AzureADHomeLinksContributor>().As<IHomeLinksContributor>().InstancePerDependency();

            // These are important as Singletons because they cache X509 certificates for performance
            builder.RegisterType<DefaultKeyJsonParser>().As<IKeyJsonParser>().SingleInstance();
            builder.RegisterType<AzureADKeyRetriever>().As<IKeyRetriever>().SingleInstance();

            builder.RegisterType<AzureADStaticContentFolders>().As<IContributesStaticContentFolders>().InstancePerDependency();

            builder.RegisterType<AzureADUserAuthenticationAction>().AsSelf().InstancePerDependency();
            builder.RegisterType<AzureADUserAuthenticatedAction>().AsSelf().InstancePerDependency();

            builder.RegisterType<AzureADAuthenticationProvider>()
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