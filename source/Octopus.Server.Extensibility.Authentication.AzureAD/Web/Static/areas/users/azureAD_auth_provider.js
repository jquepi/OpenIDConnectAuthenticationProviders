var providerName = "Azure AD";

function azureADAuthProvider(octopusClient) {
    this.octopusClient = octopusClient;

    this.name = providerName;
    this.linkHtml = '<a><div class="external-provider-button aad-button"><img src="' + octopusClient.resolve("~/images/microsoft_signin_buttons/microsoft-logo.svg") + '" /><div>Sign in with Microsoft</div></div></a>';

    this.signIn = function (authLink, redirectAfterLoginToLink, success) {
        console.log(this.name + " clicked");

        var redirectToLink = function (externalProviderLink) {
            window.location.href = externalProviderLink.ExternalAuthenticationUrl;
        };
        var postData = {
            ApiAbsUrl: octopusClient.resolve("~/"),
            RedirectAfterLoginTo: redirectAfterLoginToLink
        };

         octopusClient.post(
             authLink, 
             postData, 
             redirectToLink
         );
    };

    return {
        Name: this.name,
        LinkHtml: this.linkHtml,
        SignIn: this.signIn
    };
}

console.log("Registering " + providerName + " auth provider");
window.Octopus.registerExtension(providerName, "auth_provider", azureADAuthProvider);