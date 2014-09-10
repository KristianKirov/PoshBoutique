poshBoutiqueApp.directive('emailSubscriptionForm', function () {
    return {
        restrict: 'E',
        replace: false,
        templateUrl: 'partials/emailSubscriptionForm.html',
        controller: function ($scope, subscriptionsService, $timeout) {
            $scope.subscribed = false;
            $scope.subscribe = function () {
                $scope.error = null;
                subscriptionsService.subscribe($scope.email)
                    .success(function () {
                        $scope.subscribed = true;

                        $timeout(function () {
                            $scope.error = null;
                            $scope.subscribed = false;
                            $scope.email = null;
                        }, 5000);
                    })
                    .error(function (data, status) {
                        $scope.subscribed = false;
                        if (status == 409) {
                            $scope.error = "Този email вече съществува!";
                        }
                        else {
                            $scope.error = "Грешка!";
                        }
                    });
            };
        }
    };
});