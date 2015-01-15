poshBoutiqueApp.factory("authenticationFlow", ["$state", "$stateParams", "$window", function ($state, $stateParams, $window) {
    var _getReturnData = function (state, stateParams) {
        var stateData = {
            sn: state.name,
            sp: stateParams
        };

        return angular.toJson(stateData);
    };

    return {
        getReturnData: function (state, stateParams) {
            return _getReturnData(state, stateParams);
        },
        getCurrentReturnData: function () {
            return _getReturnData($state.current, $stateParams);
        },
        goToState: function (returnData, replaceUrl) {
            var stateData = angular.fromJson(returnData);

            if ($state.$current.name != stateData.sn || !equalForKeys(stateData.sp, $stateParams)) {
                debugger;
                if (replaceUrl) {
                    var stateUrl = $state.href(stateData.sn, stateData.sp);
                    $window.location = "/" + stateUrl;
                }
                else {
                    $state.transitionTo(stateData.sn, stateData.sp);
                }
            }
            else {
                $state.forceReload();
            }
        }
    };
}]);

poshBoutiqueApp.factory("authenticateModal", ["$modal", "authenticationFlow", function ($modal, authenticationFlow) {
    return {
        open: function (returnData) {
            if (!returnData) {
                debugger;
                returnData = authenticationFlow.getCurrentReturnData();
            }

            $modal.open({
                templateUrl: "partials/authenticateModal.html",
                resolve: {
                    externalLogins: ["accountDataService", function (accountDataService) {
                        return accountDataService.getExternalLogins(returnData, true);
                    }]
                },
                controller: ["$scope", "externalLogins", "accountDataService", "authenticationStorage", "currentUser", "$state", "$window", function ($scope, externalLogins, accountDataService, authenticationStorage, currentUser, $state, $window) {
                    $scope.login = {};
                    $scope.register = {};

                    $scope.externalLogins = externalLogins;

                    var onUserAuthenticated = function (accessToken) {
                        debugger;
                        if (accessToken) {
                            var rememberMe = $scope.login.keepMeLoggedIn;
                            currentUser.login(accessToken, rememberMe);
                            $scope.$destroy();
                            $scope.$close();

                            //if ($window.location.href == returnUrl) {
                            //    //$state.reload();
                            //    $state.forceReload();
                            //}
                            //else {
                            //    $window.location = returnUrl;
                            //}
                            debugger;
                            authenticationFlow.goToState(returnData);
                        }
                    }

                    $scope.registerUser = function () {
                        $scope.registerError = null;
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
                        })
                        .error(function (data, status, headers, config) {
                            if (status == 400) {
                                $scope.registerError = "Потребител с този Email вече съществува.";
                            }
                            else {
                                $scope.registerError = "Възникна неизвестна грешка.";
                            }
                        });
                    }

                    $scope.loginUser = function () {
                        $scope.loginError = null;
                        accountDataService.login({
                            grant_type: "password",
                            username: $scope.login.user.email,
                            password: $scope.login.user.password
                        })
                        .success(function (data) {
                            onUserAuthenticated(data.access_token);
                        })
                        .error(function (data, status, headers, config) {
                            if (status == 400) {
                                $scope.loginError = "Потребител с този Email не е намерен.";
                            }
                            else {
                                $scope.loginError = "Възникна неизвестна грешка.";
                            }
                        });
                    };

                    $scope.loginWithProvider = function (publicProvider) {
                        $window.location = publicProvider.url;
                    };
                }]
            });
        }
    }
}]);