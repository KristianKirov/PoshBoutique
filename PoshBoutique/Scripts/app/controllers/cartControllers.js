poshBoutiqueApp.controller('cartController', function ($scope, shoppingCart, $state) {
    $scope.cart = shoppingCart;
    $scope.navigateToNextState = function () {
        if ($scope.currentStepIndex < (shoppingCart.steps.length - 1)) {
            var nextStep = shoppingCart.steps[$scope.currentStepIndex + 1];
            $state.go(nextStep.stateName);
        }
    };

    $scope.setCurrentStepIndex = function (currentStateName) {
        $scope.currentStepIndex = shoppingCart.getStepIndex($state.$current.name);
    }
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

poshBoutiqueApp.controller('cartAddressController', function ($scope, $state) {
    $scope.setCurrentStepIndex($state.$current.name);
});

poshBoutiqueApp.controller('cartDeliveryController', function ($scope, $state) {
    $scope.setCurrentStepIndex($state.$current.name);
});

poshBoutiqueApp.controller('cartPaymentController', function ($scope, $state) {
    $scope.setCurrentStepIndex($state.$current.name);
});

poshBoutiqueApp.controller('cartConfirmationController', function ($scope, $state) {
    $scope.setCurrentStepIndex($state.$current.name);
});