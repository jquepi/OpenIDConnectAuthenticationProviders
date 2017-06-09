var providerName = "Azure AD";

function azureADAuthProvider(octopusClient, provider, redirectAfterLoginToLink, onError) {
    this.octopusClient = octopusClient;
    this.provider = provider;

    this.linkHtml = '<a><div class="aad-button"><img src="' + octopusClient.resolve("~/images/microsoft_signin_buttons/microsoft-logo.svg") + '" /><div>Sign in with Microsoft</div></div></a>';

    this.signIn = function () {
        console.log("Signing in using " + providerName + " provider");

        var authUri = this.provider.Links.Authenticate;
        var redirectToLink = function (externalProviderLink) {
            window.location.href = externalProviderLink.ExternalAuthenticationUrl;
        };
        var postData = {
            ApiAbsUrl: octopusClient.resolve("~/"),
            RedirectAfterLoginTo: redirectAfterLoginToLink
        };

        octopusClient.post(authUri, postData)
                    .then(redirectToLink, onError);
    };

    return {
        Name: providerName,
        LinkHtml: this.linkHtml,
        SignIn: this.signIn
    };
}

console.log("Registering " + providerName + " auth provider");
window.Octopus.registerExtension(providerName, "auth_provider", azureADAuthProvider);