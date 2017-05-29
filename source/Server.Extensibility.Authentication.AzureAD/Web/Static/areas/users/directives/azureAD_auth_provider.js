var module = angular.module('octopusApp.users.azureAD');

module.directive("azureAuthProvider", function () {
    return {
        restrict: 'E',
        replace: true,
        transclude: true,
        controller: 'AzureADAuthController',
        scope: {
            provider: '=',
            isSubmitting: '=',
            handleSignInError: '=',
            shouldAutoLogin: '='
        },
        template: '<a ng-click="signIn()"><div class="external-provider-button aad-button"><img src="{{ \'~/images/microsoft_signin_buttons/microsoft-logo.svg\' | resolveLink }}"/><div>Sign in with Microsoft</div></div></a>'
    };
});
