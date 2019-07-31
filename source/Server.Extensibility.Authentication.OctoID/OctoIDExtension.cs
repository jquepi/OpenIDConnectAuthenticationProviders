using Autofac;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Server.Extensibility.Authentication.Extensions;
using Octopus.Server.Extensibility.Authentication.Extensions.Identities;
using Octopus.Server.Extensibility.Extensions.Infrastructure;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Server.Extensibility.Extensions.Infrastructure.Web.Content;
using Octopus.Server.Extensibility.Extensions.Mappings;
using Octopus.Server.Extensibility.Authentication.OctoID.Configuration;
using Octopus.Server.Extensibility.Authentication.OctoID.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OctoID.Issuer;
using Octopus.Server.Extensibility.Authentication.OctoID.Tokens;
using Octopus.Server.Extensibility.Authentication.OctoID.Web;
using Octopus.Server.Extensibility.Authentication.OctoID.Identities;
using Octopus.Server.Extensibility.Extensions;
using Octopus.Server.Extensibility.HostServices.Web;

namespace Octopus.Server.Extensibility.Authentication.OctoID
{
    [OctopusPlugin("OctopusID", "Octopus Deploy")]
    public class OctoIDExtension : OpenIDConnectExtension, IOctopusExtension
    {
        public override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<OctoIDDatabaseInitializer>().As<IExecuteWhenDatabaseInitializes>().InstancePerDependency();
            builder.RegisterType<OctoIDPrincipalToUserResourceMapper>().As<IOctoIDPrincipalToUserResourceMapper>().InstancePerDependency();
            builder.RegisterType<OctoIDConfigurationMapping>().As<IConfigurationDocumentMapper>().InstancePerDependency();

            builder.RegisterType<OctoIDIdentityCreator>().As<IOctoIDIdentityCreator>().SingleInstance();

            builder.RegisterType<OctoIDConfigurationStore>()
                .As<IOctoIDConfigurationStore>()
                .InstancePerDependency();
            builder.RegisterType<OctoIDConfigurationSettings>()
                .As<IOctoIDConfigurationSettings>()
                .As<IHasConfigurationSettings>()
                .As<IHasConfigurationSettingsResource>()
                .As<IContributeMappings>()
                .InstancePerDependency();
            builder.RegisterType<OctoIDConfigureCommands>()
                .As<IContributeToConfigureCommand>()
                .InstancePerDependency();

            builder.RegisterType<OctoIDAuthorizationEndpointUrlBuilder>().As<IOctoIDAuthorizationEndpointUrlBuilder>().InstancePerDependency();
            builder.RegisterType<OctoIDAuthTokenHandler>().As<IOctoIDAuthTokenHandler>().InstancePerDependency();

            builder.RegisterType<OctoIDHomeLinksContributor>().As<IHomeLinksContributor>().InstancePerDependency();

            builder.RegisterType<OctoIDLoginParametersHandler>().As<ICanHandleLoginParameters>().InstancePerDependency();

            // These are important as Singletons because they cache X509 certificates for performance
            builder.RegisterType<DefaultKeyJsonParser>().As<IKeyJsonParser>().SingleInstance();
            builder.RegisterType<OctoIDKeyRetriever>().As<IOctoIDKeyRetriever>().SingleInstance();

            builder.RegisterType<OctoIDStaticContentFolders>().As<IContributesStaticContentFolders>().InstancePerDependency();

            builder.RegisterType<OctoIDUserAuthenticationAction>().AsSelf().InstancePerDependency();
            builder.RegisterType<OctoIDUserAuthenticatedAction>().AsSelf().InstancePerDependency();

            builder.RegisterType<OctoIDAuthenticationProvider>()
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