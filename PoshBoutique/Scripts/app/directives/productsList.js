poshBoutiqueApp.directive('productsList', function () {
    return {
        restrict: 'E',
        scope: {
            listData: '='
        },
        templateUrl: 'partials/productsList.html',
        controller: function ($scope) {
        }
    };
});