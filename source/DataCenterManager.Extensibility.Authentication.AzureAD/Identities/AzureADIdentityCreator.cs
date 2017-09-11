﻿using Octopus.Node.Extensibility.Authentication.OpenIDConnect.Identities;

namespace Octopus.DataCenterManager.Extensibility.Authentication.AzureAD.Identities
{
    public class AzureADIdentityCreator : IdentityCreator, IAzureADIdentityCreator
    {
        protected override string ProviderName => AzureADAuthenticationProvider.ProviderName;
    }

    public interface IAzureADIdentityCreator : IIdentityCreator
    { }
}