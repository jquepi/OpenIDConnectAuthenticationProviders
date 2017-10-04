using Autofac;
using Octopus.Node.Extensibility.Authentication.Extensions;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect;
using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Certificates;
using Octopus.Node.Extensibility.Extensions;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Configuration;
using Octopus.Node.Extensibility.Extensions.Infrastructure.Web.Content;
using Octopus.Node.Extensibility.HostServices.Web;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Configuration;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Identities;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Infrastructure;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Issuer;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Tokens;
using Octopus.Server.Extensibility.Authentication.DataCenterManager.Web;
using Octopus.Server.Extensibility.HostServices.Web;

namespace Octopus.Server.Extensibility.Authentication.DataCenterManager
{
    [OctopusPlugin("DataCenterManager", "Octopus Deploy")]
    public class DataCenterManagerExtension : OpenIDConnectExtension, IOctopusExtension
    {
        public override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<DataCenterManagerPrincipalToUserResourceMapper>().As<IDataCenterManagerPrincipalToUserResourceMapper>().InstancePerDependency();
            builder.RegisterType<DataCenterManagerConfigurationMapping>().As<IConfigurationDocumentMapper>().InstancePerDependency();

            builder.RegisterType<DataCenterManagerIdentityCreator>().As<IDataCenterManagerIdentityCreator>().SingleInstance();

            builder.RegisterType<DataCenterManagerConfigurationStore>()
                .As<IDataCenterManagerConfigurationStore>()
                .As<IHasConfigurationSettings>()
                .InstancePerDependency();
            builder.RegisterType<DataCenterManagerConfigureCommands>()
                .As<IContributeToConfigureCommand>()
                .InstancePerDependency();

            builder.RegisterType<UrlEncoder>().As<IUrlEncoder>().InstancePerDependency();
            
            builder.RegisterType<DataCenterManagerAuthorizationEndpointUrlBuilder>().As<IDataCenterManagerAuthorizationEndpointUrlBuilder>().InstancePerDependency();
            builder.RegisterType<DataCenterManagerAuthTokenHandler>().As<IDataCenterManagerAuthTokenHandler>().InstancePerDependency();

            builder.RegisterType<DataCenterManagerHomeLinksContributor>().As<IHomeLinksContributor>().InstancePerDependency();

            // These are important as Singletons because they cache X509 certificates for performance
            builder.RegisterType<DefaultKeyJsonParser>().As<IKeyJsonParser>().SingleInstance();
            builder.RegisterType<DataCenterManagerKeyRetriever>().As<IKeyRetriever>().SingleInstance();

            builder.RegisterType<DataCenterManagerStaticContentFolders>().As<IContributesStaticContentFolders>().InstancePerDependency();

            builder.RegisterType<DataCenterManagerUserAuthenticationAction>().AsSelf().InstancePerDependency();
            builder.RegisterType<DataCenterManagerUserAuthenticatedAction>().AsSelf().InstancePerDependency();

            builder.RegisterType<DataCenterManagerAuthenticationProvider>()
                .As<IAuthenticationProvider>()
                .As<IAuthenticationProviderWithGroupSupport>()
                .As<IContributesCSS>()
                .As<IContributesJavascript>()
                .AsSelf()
                .InstancePerDependency();
        }
    }
}