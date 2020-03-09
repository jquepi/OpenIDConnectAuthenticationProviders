﻿using System.ComponentModel;
using Octopus.Data.Resources;
using Octopus.Data.Resources.Attributes;
using Octopus.Server.Extensibility.Authentication.OpenIDConnect.Common.Configuration;

namespace Octopus.Server.Extensibility.Authentication.OctopusID.Configuration
{
    [Description("Sign in to your Octopus Server with an Octopus ID. [Learn more](https://g.octopushq.com/AuthOctopusID).")]
    public class OctopusIDConfigurationResource : OpenIDConnectConfigurationWithClientSecretResource
    {
        [ReadOnly(true)]
        public override string Issuer { get; set; }
        
        /// <summary>
        /// NOTE: the following properties are here to control the order they appear on the settings page
        /// </summary>
        
        [ReadOnly(true)]
        public override string ClientId { get; set; }
        [ReadOnly(true)]
        public override SensitiveValue ClientSecret { get; set; }
        [ReadOnly(true)]
        public override bool? AllowAutoUserCreation { get; set; }

        [ReadOnly(true)]
        public new bool IsEnabled { get; set; }
    }
}