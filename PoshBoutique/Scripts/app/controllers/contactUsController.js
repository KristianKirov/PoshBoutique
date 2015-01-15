poshBoutiqueApp.controller('contactUsController', ["$scope", "currentUser", "feedbackService", function ($scope, currentUser, feedbackService) {
    $scope.submitted = false;
    $scope.error = false;

    if (currentUser.isAuthenticated) {
        $scope.email = currentUser.email;
        $scope.fullName = currentUser.firstName + " " + currentUser.lastName;
    }

    $scope.submitContactForm = function () {
        feedbackService.submit({ email: $scope.email, name: $scope.fullName, message: $scope.message })
            .success(function () {
                $scope.submitted = true;
                $scope.error = false;
            })
            .error(function () {
                $scope.submitted = false;
                $scope.error = true;
            });
    };
}]);