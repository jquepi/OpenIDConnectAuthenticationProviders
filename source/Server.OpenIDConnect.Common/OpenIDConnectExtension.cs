using Autofac;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Infrastructure;

namespace Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common
{
    public abstract class OpenIDConnectExtension
    {
        public virtual void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PrincipalToUserResourceMapper>().As<IPrincipalToUserResourceMapper>().InstancePerDependency();
        }
    }
}