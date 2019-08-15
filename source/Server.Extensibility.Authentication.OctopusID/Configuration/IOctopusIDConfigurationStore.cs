﻿using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctopusID.Configuration
{
    public interface IOctopusIDConfigurationStore : IOpenIDConnectWithClientSecretConfigurationStore<OctopusIDConfiguration>, 
        IOpenIDConnectConfigurationWithRoleStore<OctopusIDConfiguration>
    {
    }
}