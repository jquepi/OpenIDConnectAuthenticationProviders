var providerName = "Data Center Manager";

function DataCenterManagerAuthProvider(octopusClient, provider, redirectAfterLoginToLink, onError) {

    this.linkHtml = '<a><div class="dcm-button"><img src="' + octopusClient.resolve("~/images/dcm_signin_buttons/dcm-logo.svg") + '" /><div>Sign in with Data Center Manager</div></div></a>';

    this.signIn = function () {
        console.log("Signing in using " + providerName + " provider");

        var authUri = provider.Links.Authenticate;
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
window.Octopus.registerExtension(providerName, "auth_provider", DataCenterManagerAuthProvider);
