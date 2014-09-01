﻿poshBoutiqueApp.directive('currentUserNav', function () {
    return {
        restrict: 'EA',
        replace: true,
        templateUrl: 'partials/currentUserNav.html',
        controller: function ($scope, authenticateModal, currentUser, $state) {
            $scope.login = function () {
                authenticateModal.open();
            };

            $scope.logout = function () {
                currentUser.logout();
                debugger;
                if ($state.$current && $state.$current.data && $state.$current.data.authenticated) {
                    $state.go("home");
                }
                else {
                    $state.reload();
                }
            };

            $scope.hasData = function () {
                return currentUser.isAuthenticated && !!currentUser.firstName
            };

            $scope.currentUser = currentUser;
        }
    };
});