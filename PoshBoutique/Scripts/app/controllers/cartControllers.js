poshBoutiqueApp.controller('cartController', function ($scope, shoppingCart) {
    $scope.cart = shoppingCart;
});

poshBoutiqueApp.controller('cartOrderController', function ($scope, shoppingCart) {
    shoppingCart.validateCurrentDisplayStateOrRedirect();
});

poshBoutiqueApp.controller('cartAddressController', function ($scope, shoppingCart) {
    shoppingCart.validateCurrentDisplayStateOrRedirect();
});

poshBoutiqueApp.controller('cartPaymentController', function ($scope, shoppingCart) {
    shoppingCart.validateCurrentDisplayStateOrRedirect();
});

poshBoutiqueApp.controller('cartConfirmationController', function ($scope, shoppingCart) {
    shoppingCart.validateCurrentDisplayStateOrRedirect();
});