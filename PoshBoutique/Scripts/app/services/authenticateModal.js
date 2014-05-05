poshBoutiqueApp.factory("authenticateModal", function ($modal, $window) {
    return {
        open: function (returnUrl) {
            returnUrl = returnUrl || $window.location.href;

            $modal.open({
                templateUrl: "partials/authenticateModal.html",
                resolve: {
                    externalLogins: function (accountDataService) {
                        return accountDataService.getExternalLogins(returnUrl, true);
                    }
                },
                controller: function ($scope, externalLogins, accountDataService, authenticationStorage, currentUser) {
                    $scope.login = {};
                    $scope.register = {};

                    $scope.externalLogins = externalLogins;

                    var onUserAuthenticated = function (accessToken) {
                        if (accessToken) {
                            authenticationStorage.setAccesToken(accessToken, $scope.login.keepMeLoggedIn);
                            currentUser.loadData();
                            $scope.$close(true);

                            if ($window.location.href == returnUrl) {
                                $window.location.reload();
                            }
                            else {
                                $window.location = returnUrl;
                            }
                        }
                    }

                    $scope.registerUser = function () {
                        accountDataService.register({
                            email: $scope.register.user.email,
                            firstName: $scope.register.user.firstName,
                            lastName: $scope.register.user.lastName,
                            password: $scope.register.user.password,
                            confirmPassword: $scope.register.user.confirmPassword,
                        })
                        .success(function () {
                            accountDataService.login({
                                grant_type: "password",
                                username: $scope.register.user.email,
                                password: $scope.register.user.password
                            })
                            .success(function (data) {
                                onUserAuthenticated(data.access_token);
                            });
                        });
                    }

                    $scope.loginUser = function () {
                        accountDataService.login({
                            grant_type: "password",
                            username: $scope.login.user.email,
                            password: $scope.login.user.password
                        })
                        .success(function (data) {
                            onUserAuthenticated(data.access_token);
                        });
                    };

                    $scope.loginWithProvider = function (publicProvider) {
                        $window.location = publicProvider.url;
                    };
                }
            });
        }
    }
});