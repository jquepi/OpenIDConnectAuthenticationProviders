function googleAuthProvider(octopusClient) {
    this.octopusClient = octopusClient;

    this.name = "Google Apps";
    this.linkHtml = '<a><div class="external-provider-button googleapps-button"><img src="' + octopusClient.resolve("~/images/google_signin_buttons/icon-google.svg") + '" /><div>Sign in with Google</div></div></a>';

    this.signIn = function (authLink, redirectAfterLoginToLink, success) {

        var redirectToLink = function (externalProviderLink) {
            window.location.href = externalProviderLink.ExternalAuthenticationUrl;
        };
        var postData = {
            ApiAbsUrl: octopusClient.resolve("~/"),
            RedirectAfterLoginTo: redirectAfterLoginToLink
        };

        octopusClient.post(authLink, postData)
                        .then(redirectToLink);
    };

    return {
        Name: this.name,
        LinkHtml: this.linkHtml,
        SignIn: this.signIn
    };
}

console.log("Registering Google Apps auth provider");
window.Octopus.registerExtension("Google Apps", "auth_provider", googleAuthProvider);