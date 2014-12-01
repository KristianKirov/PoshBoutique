poshBoutiqueApp.controller('accountController', function ($scope, currentUser) {
    $scope.user = currentUser;
});

poshBoutiqueApp.controller('accountEditController', function ($scope, manageInfo, addressInfo, ordersDataService, accountDataService, $timeout, $window) {
    $scope.changePasswordModel = {};
    $scope.setPasswordModel = {};
    $scope.addressInfo = addressInfo;

    $scope.saveUserAddress = function () {
        if (addressForm.$invalid) {
            return;
        }

        ordersDataService.setAddressInfo($scope.addressInfo).
            success(function () {
                $scope.addressSuccessfullySaved = true;

                $timeout(function () {
                    $scope.addressSuccessfullySaved = false;
                }, 5000);
            }).
            error(function () {
                $scope.addressErrorOnSave = true;

                $timeout(function () {
                    $scope.addressErrorOnSave = false;
                }, 5000);
            });
    }

    var findExternalLogin = function (loginProviderName) {
        for (var i = 0; i < manageInfo.externalLoginProviders.length; i++) {
            var externalLogin = manageInfo.externalLoginProviders[i];
            if (externalLogin.name == loginProviderName) {
                return externalLogin;
            }
        }
    };

    var hasLocalLogin = false;
    for (var i = 0; i < manageInfo.logins.length; i++) {
        var login = manageInfo.logins[i];

        if (login.loginProvider == manageInfo.localLoginProvider) {
            hasLocalLogin = true;
        }
        else {
            debugger;
            var externalLogin = findExternalLogin(login.loginProvider);
            externalLogin.currentLoginData = login;
        }
    }

    debugger;
    $scope.logins = manageInfo.externalLoginProviders;
    $scope.hasLocalLogin = hasLocalLogin;

    var loginsCount = manageInfo.logins.length;
    $scope.canRemoveLogin = function () {
        return loginsCount > 1;
    };

    $scope.removeLocalLogin = function () {
        $scope.removeLogin({ name: manageInfo.localLoginProvider, currentLoginData: { loginProvider: manageInfo.localLoginProvider, providerKey: $scope.user.email } });
    };

    $scope.removeLogin = function (loginProvider) {
        if (!$scope.canRemoveLogin()) {
            return;
        }
        debugger;
        var loginToRemove = loginProvider.currentLoginData;
        loginProvider.currentLoginData = null;
        loginsCount--;
        if (loginProvider.name == manageInfo.localLoginProvider) {
            $scope.hasLocalLogin = false;
        }

        accountDataService.removeLogin(loginToRemove).
            error(function () {
                loginProvider.currentLoginData = loginToRemove;
                loginsCount++;

                if (loginProvider.name == manageInfo.localLoginProvider) {
                    $scope.hasLocalLogin = true;
                }
            });
    };

    $scope.changePassword = function () {
        if ($scope.changePasswordForm.$invalid || !$scope.hasLocalLogin) {
            return;
        }

        accountDataService.changePassword({
                oldPassword: $scope.changePasswordModel.oldPassword,
                newPassword: $scope.changePasswordModel.newPassword,
                confirmPassword: $scope.changePasswordModel.confirmPassword
            }).
            success(function () {
                $scope.passwordSuccessfullyChanged = true;

                $timeout(function () {
                    $scope.passwordSuccessfullyChanged = false;
                }, 5000);
            }).
            error(function () {
                $scope.passwordErrorOnChange = true;

                $timeout(function () {
                    $scope.passwordErrorOnChange = false;
                }, 5000);
            });

    };

    $scope.addLogin = function (loginProvider) {
        if (loginProvider.name == manageInfo.localLoginProvider) {
            return;
        }

        $window.location = loginProvider.url;
    };

    $scope.setPassword = function () {
        if (newPasswordForm.$invalid || $scope.hasLocalLogin) {
            return;
        }

        accountDataService.setPassword({ newPassword: $scope.setPasswordModel.newPassword, confirmPassword: $scope.setPasswordModel.confirmPassword }).
            success(function () {
                $scope.hasLocalLogin = true;
                loginsCount++;
                $scope.passwordSuccessfullySet = true;

                $timeout(function () {
                    $scope.passwordSuccessfullySet = false;
                }, 5000);
            }).
            error(function () {
                $scope.passwordErrorOnSet = true;

                $timeout(function () {
                    $scope.passwordErrorOnSet = false;
                }, 5000);
            });
    };

    //todo: --can remove logins, --add login, --set password, --change password, --remove login (remove password)
});

poshBoutiqueApp.controller('accountOrdersController', function ($scope, userOrders, ordersDataService) {
    $scope.userOrders = userOrders;

    $scope.orderClicked = function (clickedOrder) {
        if (!clickedOrder.visible && !clickedOrder.items) {
            ordersDataService.orderItems(clickedOrder.id)
                .success(function (orderItems) {
                    clickedOrder.items = orderItems;
                });
        }

        if (!clickedOrder.visible && !clickedOrder.history) {
            ordersDataService.orderHistory(clickedOrder.id)
                .success(function (orderHistory) {
                    clickedOrder.history = orderHistory;
                });
        }
    }
});