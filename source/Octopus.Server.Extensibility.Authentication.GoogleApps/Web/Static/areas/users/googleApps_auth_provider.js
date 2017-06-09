var providerName = "Google Apps";

function googleAuthProvider(octopusClient, provider, redirectAfterLoginToLink, onError) {
    this.octopusClient = octopusClient;
    this.provider = provider;

    this.linkHtml = '<a><div class="googleapps-button"><img src="' + octopusClient.resolve("~/images/google_signin_buttons/icon-google.svg") + '" /><div>Sign in with Google</div></div></a>';

    this.signIn = function () {

        console.log("Signing in using " + providerName + " provider");

        var authLink = this.provider.Links.Authenticate;
        var redirectToLink = function (externalProviderLink) {
            window.location.href = externalProviderLink.ExternalAuthenticationUrl;
        };
        var postData = {
            ApiAbsUrl: octopusClient.resolve("~/"),
            RedirectAfterLoginTo: redirectAfterLoginToLink
        };

        octopusClient.post(authLink, postData)
                    .then(redirectToLink, onError);
    };

    return {
        Name: providerName,
        LinkHtml: this.linkHtml,
        SignIn: this.signIn
    };
}

console.log("Registering " + providerName + " auth provider");
window.Octopus.registerExtension(providerName, "auth_provider", googleAuthProvider);