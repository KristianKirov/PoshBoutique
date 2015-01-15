poshBoutiqueApp.directive('configurePurchaseForm', function () {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            product: '='
        },
        templateUrl: 'partials/configurePurchaseForm.html',
        controller: ["$scope", "shoppingCart", "$state", "$timeout", function ($scope, shoppingCart, $state, $timeout) {
            $scope.selectedSize = null;
            $scope.selectedColor = null;
            $scope.quantity = 0;
            $scope.availableQuantity = 0;
            $scope.articleAddedInCart = false;
            $scope.noQuantity = false;

            var totalQuantity = 0;
            if ($scope.product.sizes) {
                for (var i = 0; i < $scope.product.sizes.length; i++) {
                    var currentSize = $scope.product.sizes[i];
                    if (currentSize.quantity) {
                        totalQuantity += currentSize.quantity;
                    }
                }
            }
            $scope.noQuantity = !totalQuantity;

            var setQuantity = function () {
                if ($scope.quantity > $scope.availableQuantity) {
                    $scope.quantity = $scope.availableQuantity;
                }

                if ($scope.quantity < 0) {
                    $scope.quantity = 0;
                }
            };

            $scope.selectedSizeChanged = function () {
                $scope.selectedColor = $scope.hasSingleColor() ? $scope.selectedSize.colors[0] : null;
                $scope.availableQuantity = $scope.getAvailableQuantity();
                setQuantity();
            };

            $scope.hasColors = function () {
                if (!$scope.selectedSize) {
                    return false;
                }

                return $scope.selectedSize.colors && $scope.selectedSize.colors.length > 0;
            };

            $scope.hasSingleColor = function () {
                return $scope.hasColors() && $scope.selectedSize.colors.length == 1;
            };

            $scope.selectedColorChanged = function () {
                $scope.availableQuantity = $scope.getAvailableQuantity();
                setQuantity();
            };

            $scope.getAvailableQuantity = function () {
                if (!$scope.selectedSize) {
                    return 0;
                }

                if ($scope.hasColors($scope.selectedSize)) {
                    if (!$scope.selectedColor) {
                        return 0;
                    }

                    if (!$scope.selectedColor.quantity) {
                        return 0;
                    }

                    return $scope.selectedColor.quantity;
                }
                else {
                    if (!$scope.selectedSize.quantity) {
                        return 0;
                    }

                    return $scope.selectedSize.quantity;
                }
            };

            $scope.addToCart = function (navigateToCart) {
                var selectedColor = $scope.hasColors() ? $scope.selectedColor : null;
                shoppingCart.addItemToCart($scope.product, $scope.quantity, $scope.selectedSize, selectedColor);

                if (navigateToCart) {
                    $state.go("cart.order");
                }
                else {
                    $scope.articleAddedInCart = true;

                    $timeout(function () {
                        $scope.articleAddedInCart = false;
                    }, 5000);
                }
            };
        }]
    }
});