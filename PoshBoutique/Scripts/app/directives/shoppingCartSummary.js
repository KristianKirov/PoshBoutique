    poshBoutiqueApp.directive('shoppingCartSummary', function () {
    return {
        restrict: 'EA',
        replace: true,
        templateUrl: 'partials/shoppingCartSummary.html',
        controller: function ($scope, shoppingCart) {
            $scope.cart = shoppingCart;
        }
    };
});