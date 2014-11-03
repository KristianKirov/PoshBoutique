poshBoutiqueApp.controller('cartController', function ($scope, shoppingCart, $state) {
    $scope.currentStepIndex = -1;
    $scope.cart = shoppingCart;

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

    $scope.setCurrentStepIndex = function (currentStateName) {
        var newStepIndex = shoppingCart.getStepIndex($state.$current.name);
        //if (newStepIndex <= 1 + $scope.currentStepIndex) {
            $scope.currentStepIndex = newStepIndex;
        //}
        //else {
        //    navigateToStepWithIndex(0);
        //}
    };

    $scope.isStepActive = function (stepIndex) {
        return stepIndex <= $scope.currentStepIndex;
    };

    $scope.navigateToStepWithIndexSafe = function (stepIndex) {
        if (stepIndex < $scope.currentStepIndex) {
            navigateToStepWithIndex(stepIndex);
        }
    };
});

poshBoutiqueApp.controller('cartOrderController', function ($scope, $state, ordersDataService, shoppingCart) {
    $scope.setCurrentStepIndex($state.$current.name);

    var orderedItemsSimple = [];
    for (var i = 0; i < shoppingCart.items.length; i++) {
        var orderedItem = shoppingCart.items[i];
        var orderedItemSimple = {
            articleId: orderedItem.id,
            sizeId: orderedItem.size.id,
            quantity: orderedItem.quantity,
            price: orderedItem.price
        };

        if (orderedItem.color) {
            orderedItemSimple.colorId = orderedItem.color.id;
        }
        orderedItemsSimple.push(orderedItemSimple);
    }

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

poshBoutiqueApp.controller('cartAddressController', function ($scope, $state, addressInfo, ordersDataService) {
    $scope.setCurrentStepIndex($state.$current.name);
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

poshBoutiqueApp.controller('cartPaymentController', function ($scope, $state) {
    $scope.setCurrentStepIndex($state.$current.name);
});

poshBoutiqueApp.controller('cartConfirmationController', function ($scope, $state) {
    $scope.setCurrentStepIndex($state.$current.name);
});