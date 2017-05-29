var module = angular.module('octopusApp.users.google');

module.directive("googleAuthProvider", function () {
    return {
        restrict: 'E',
        replace: true,
        controller: 'GoogleAppsAuthController',
        scope: {
            provider: '=',
            isSubmitting: '=',
            handleSignInError: '=',
            shouldAutoLogin: '='
        },
        template: '<a ng-click="signIn()"><div class="external-provider-button googleapps-button"><img src="{{ \'~/images/google_signin_buttons/icon-google.svg\' | resolveLink }}" /><div>Sign in with Google</div></div></a>'
    };
});
