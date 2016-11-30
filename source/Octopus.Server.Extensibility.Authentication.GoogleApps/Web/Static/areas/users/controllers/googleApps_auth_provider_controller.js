var module = angular.module('octopusApp.users.google');

module.controller('GoogleAppsAuthController', function ($scope, $rootScope, octopusClient, busy, $window, $location) {

    $scope.name = $scope.provider.Name;
    $scope.linkHtml = $scope.provider.LinkHtml;

    var redirectToLink = function (externalProviderLink) {
        $window.location.href = externalProviderLink.ExternalAuthenticationUrl;
    };

    $scope.resolveLink = function (link) {
        if (link)
            return octopusClient.resolve(link);
        return null;
    }

    $scope.signIn = function () {
        if ($scope.isSubmitting.busy) {
            return;
        }
        $scope.isSubmitting.promise(octopusClient.post($scope.provider.Links["Authenticate"], { ApiAbsUrl: $scope.resolveLink('~/'), RedirectAfterLoginTo: $rootScope.absoluteRedirectAfterLoginTo }).then(redirectToLink, $scope.handleSignInError));
    };

    if ($scope.shouldAutoLogin) {
        $scope.signIn();
    }
});

