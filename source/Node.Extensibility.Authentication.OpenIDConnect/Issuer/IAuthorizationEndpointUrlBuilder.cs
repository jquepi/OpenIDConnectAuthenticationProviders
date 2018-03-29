﻿namespace Octopus.Node.Extensibility.Authentication.OpenIDConnect.Issuer
{
    public interface IAuthorizationEndpointUrlBuilder
    {
        string Build(string requestDirectoryPath, IssuerConfiguration issuerConfiguration, string nonce, string state = null);
    }
}