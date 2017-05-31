using Autofac;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Infrastructure;
using Octopus.Node.Extensibility.Authentication.OpenIdConnect.Issuer;

namespace Octopus.Node.Extensibility.Authentication.OpenIdConnect
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