poshBoutiqueApp.directive('currentUserNav', function () {
    return {
        restrict: 'EA',
        replace: true,
        templateUrl: 'partials/currentUserNav.html',
        controller: function ($scope, authenticateModal, currentUser, $state) {
            $scope.login = function () {
                authenticateModal.open();
            };

            $scope.logout = function () {
                currentUser.logoutAndRedirect();
            };

            $scope.hasData = function () {
                return currentUser.isAuthenticated && !!currentUser.firstName
            };

            $scope.currentUser = currentUser;
        }
    };
});