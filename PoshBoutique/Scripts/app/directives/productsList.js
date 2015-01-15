poshBoutiqueApp.directive('productsList', function () {
    return {
        restrict: 'E',
        scope: {
            listData: '=',
            articleListParams: '='
        },
        templateUrl: 'partials/productsList.html',
        controller: ["$scope", function ($scope) {
        }]
    };
});