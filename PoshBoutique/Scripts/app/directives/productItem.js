poshBoutiqueApp.directive('productItem', function () {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            item: '=',
            categoryUrl: '@'
        },
        templateUrl: 'partials/productItem.html',
        controller: function ($scope) {
        }
    };
});