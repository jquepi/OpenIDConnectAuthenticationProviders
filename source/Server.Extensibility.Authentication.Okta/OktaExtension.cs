using Autofac;
using Octopus.Node.Extensibility.Authentication.Extensions;
using Octopus.Node.Extensibility.Authentication.Extensions.Identities;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Node.Extensibility.Extensions;
using Octopus.Node.Extensibility.Extensions.Infrastructure;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Web.Content;
using Octopus.Node.Extensibility.Extensions.Mappings;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Server.Extensibility.Authentication.Okta.Configuration;
using Octopus.Server.Extensibility.Authentication.Okta.Infrastructure;
using Octopus.Server.Extensibility.Authentication.Okta.Issuer;
using Octopus.Server.Extensibility.Authentication.Okta.Tokens;
using Octopus.Server.Extensibility.Authentication.Okta.Web;
using Octopus.Server.Extensibility.Authentication.Okta.Identities;
using Octopus.Server.Extensibility.HostServices.Web;

namespace Octopus.Server.Extensibility.Authentication.Okta
{
    [OctopusPlugin("Okta", "Octopus Deploy")]
    public class OktaExtension : OpenIDConnectExtension, IOctopusExtension
    {
        public override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<OktaDatabaseInitializer>().As<IExecuteWhenDatabaseInitializes>().InstancePerDependency();
            builder.RegisterType<OktaPrincipalToUserResourceMapper>().As<IOktaPrincipalToUserResourceMapper>().InstancePerDependency();
            builder.RegisterType<OktaConfigurationMapping>().As<IConfigurationDocumentMapper>().InstancePerDependency();

            builder.RegisterType<OktaIdentityCreator>().As<IOktaIdentityCreator>().SingleInstance();

            builder.RegisterType<OktaConfigurationStore>()
                .As<IOktaConfigurationStore>()
                .As<IOktaConfigurationStore>()
                .InstancePerDependency();
            builder.RegisterType<OktaConfigurationSettings>()
                .As<IOktaConfigurationSettings>()
                .As<IHasConfigurationSettings>()
                .As<IHasConfigurationSettingsResource>()
                .As<IContributeMappings>()
                .InstancePerDependency();
            builder.RegisterType<OktaConfigureCommands>()
                .As<IContributeToConfigureCommand>()
                .InstancePerDependency();

            builder.RegisterType<UrlEncoder>().As<IUrlEncoder>().InstancePerDependency();

            builder.RegisterType<OktaAuthorizationEndpointUrlBuilder>().As<IOktaAuthorizationEndpointUrlBuilder>().InstancePerDependency();
            builder.RegisterType<OktaAuthTokenHandler>().As<IOktaAuthTokenHandler>().InstancePerDependency();

            builder.RegisterType<OktaHomeLinksContributor>().As<IHomeLinksContributor>().InstancePerDependency();

            // These are important as Singletons because they cache X509 certificates for performance
            builder.RegisterType<DefaultKeyJsonParser>().As<IKeyJsonParser>().SingleInstance();
            builder.RegisterType<OktaKeyRetriever>().As<IOktaKeyRetriever>().SingleInstance();

            builder.RegisterType<OktaStaticContentFolders>().As<IContributesStaticContentFolders>().InstancePerDependency();

            builder.RegisterType<OktaUserAuthenticationAction>().AsSelf().InstancePerDependency();
            builder.RegisterType<OktaUserAuthenticatedAction>().AsSelf().InstancePerDependency();

            builder.RegisterType<OktaAuthenticationProvider>()
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