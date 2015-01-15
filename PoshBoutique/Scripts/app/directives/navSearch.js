poshBoutiqueApp.directive('navSearch', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: 'partials/navSearch.html',
        scope: {
        },
        controller: ["$scope", "$state", function ($scope, $state) {
            $scope.searchArticles = function (searchForm) {
                $state.go("search", { term: $scope.searchTerm });

                return true;
            };
        }]
    }
});