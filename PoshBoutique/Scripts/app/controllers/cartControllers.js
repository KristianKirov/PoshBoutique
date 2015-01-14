poshBoutiqueApp.controller('cartController', function ($scope, shoppingCart, $state) {
    $scope.currentStepIndex = -1;
    $scope.cart = shoppingCart;
    shoppingCart.clearCoupones();

    var navigateToStepWithIndex = function (stepIndex) {
        var newStep = shoppingCart.steps[stepIndex];
        if (newStep) {
            $state.go(newStep.stateName);
        }
    };

    $scope.navigateToNextState = function () {
        if ($scope.currentStepIndex < (shoppingCart.steps.length - 1)) {
            navigateToStepWithIndex($scope.currentStepIndex + 1);
        }
    };

    $scope.navigateToPrevState = function () {
        if ($scope.currentStepIndex > 0) {
            navigateToStepWithIndex($scope.currentStepIndex - 1);
        }
    };

    $scope.navigateToStepWithIndexSafe = function (stepIndex) {
        if (stepIndex < $scope.currentStepIndex) {
            navigateToStepWithIndex(stepIndex);
        }
    };

    $scope.setCurrentStepIndex = function (currentStateName) {
        var newStepIndex = shoppingCart.getStepIndex($state.$current.name);
        var canDisplayState = true;
        for (var i = 0; i <= newStepIndex; i++) {
            preceedingStep = shoppingCart.steps[i];
            if (!preceedingStep.canShow()) {
                canDisplayState = false;
                break;
            }
        }

        if (canDisplayState) {
            $scope.currentStepIndex = newStepIndex;
        }
        else {
            navigateToStepWithIndex(0);
        }
    };

    $scope.isStepActive = function (stepIndex) {
        return stepIndex <= $scope.currentStepIndex;
    };
});

poshBoutiqueApp.controller('cartOrderController', function ($scope, $state, ordersDataService, defaultCoupons) {
    $scope.setCurrentStepIndex($state.$current.name);

    $scope.cart.addCoupones(defaultCoupons);

    var orderedItemsSimple = $scope.cart.getSimpleItems();

    ordersDataService.validateOrder(orderedItemsSimple)
        .success(function () {
            $scope.isValid = true;
        })
        .error(function () {
            $scope.isValid = false;
            //TODO: show message to user!
        });

    $scope.isValid = false;
});

poshBoutiqueApp.controller('cartAddressController', function ($scope, $state, addressInfo, ordersDataService, defaultCoupons) {
    $scope.setCurrentStepIndex($state.$current.name);

    $scope.cart.addCoupones(defaultCoupons);

    if (!$scope.cart.addressInfo) {
        $scope.cart.addressInfo = addressInfo;
    }
    
    $scope.$on("$destroy", function () {
        if ($scope.addressForm.$valid) {
            ordersDataService.setAddressInfo($scope.cart.addressInfo);
        }
    });
});

poshBoutiqueApp.controller('cartDeliveryController', function ($scope, $state, deliveryMethods) {
    $scope.setCurrentStepIndex($state.$current.name);

    $scope.deliveryMethods = deliveryMethods;
});

poshBoutiqueApp.controller('cartPaymentController', function ($scope, $state, paymentMethods) {
    $scope.setCurrentStepIndex($state.$current.name);

    $scope.paymentMethods = paymentMethods;
});

poshBoutiqueApp.controller('cartConfirmationController', function ($scope, $state, ordersDataService, $window) {
    $scope.setCurrentStepIndex($state.$current.name);

    $scope.postOrder = function () {
        var simpleOrder = $scope.cart.getSimpleOrder();
        ordersDataService.validateAndSaveOrder(simpleOrder)
            .success(function (orderId) {
                var externalPaymentService = $scope.cart.selectedPaymentMethod.isExternal;

                $scope.cart.clean();

                if (externalPaymentService) {
                    $window.location = "/pay/order/" + orderId;
                }
                else {
                    $state.go("order-accepted");
                }
            })
            .error(function () {
                //TODO: show message to user!
            });
    };
});