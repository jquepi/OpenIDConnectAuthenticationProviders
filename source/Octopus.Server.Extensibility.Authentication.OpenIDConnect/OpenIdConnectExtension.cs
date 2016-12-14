using Autofac;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Infrastructure;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Issuer;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect
{
    public abstract class OpenIDConnectExtension
    {
        public virtual void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PrincipalToUserResourceMapper>().As<IPrincipalToUserResourceMapper>().InstancePerDependency();
            builder.RegisterType<IdentityProviderConfigDiscoverer>().As<IIdentityProviderConfigDiscoverer>().SingleInstance();
        }
    }
}