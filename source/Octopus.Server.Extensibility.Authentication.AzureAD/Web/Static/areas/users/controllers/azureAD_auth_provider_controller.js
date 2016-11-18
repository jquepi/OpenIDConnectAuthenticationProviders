var module = angular.module('octopusApp.users.azureAD');

module.controller('AzureADAuthController', function ($scope, $rootScope, octopusClient, busy, $window) {

    var isSubmitting = $scope.isSubmitting = busy.create();

    $scope.name = $scope.provider.Name;

    $scope.linkHtml = $scope.provider.LinkHtml;

    var redirectToLink = function (externalProviderLink) {
        $window.location.href = externalProviderLink.ExternalAuthenticationUrl;
    };

    $scope.signIn = function() {
        if (isSubmitting.busy) {
            return;
        }

        isSubmitting.promise(octopusClient.post($scope.provider.Links["Authenticate"], { ApiAbsUrl: octopusClient.endpoint, RedirectAfterLoginTo: $rootScope.redirectAfterLoginTo }).then(redirectToLink));
    };

    if ($scope.shouldAutoLogin) {
        $scope.signIn();
    }
});

