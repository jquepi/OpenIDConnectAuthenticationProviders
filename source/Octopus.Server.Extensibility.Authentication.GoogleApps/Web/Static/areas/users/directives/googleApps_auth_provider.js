var module = angular.module('octopusApp.users.google');

module.directive("googleAuthProvider", function () {
    return {
        restrict: 'E',
        replace: true,
        transclude: true,
        controller: 'GoogleAppsAuthController',
        scope: {
            provider: '=',
            shouldAutoLogin: '='
        },
        template: '<a ng-click="signIn()"><div class="external-provider-button googleapps-button"><img src="images/google_signin_buttons/icon-google.svg"><div>Sign in with Google</div></div></a>'
    };
});
