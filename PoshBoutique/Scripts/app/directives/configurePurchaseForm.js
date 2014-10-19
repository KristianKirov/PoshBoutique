poshBoutiqueApp.directive('configurePurchaseForm', function () {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            product: '='
        },
        templateUrl: 'partials/configurePurchaseForm.html',
        controller: function ($scope, shoppingCart, $state) {
            $scope.selectedSize = null;
            $scope.selectedColor = null;
            $scope.quantity = 0;
            $scope.availableQuantity = 0;

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
            };
        }
    }
});