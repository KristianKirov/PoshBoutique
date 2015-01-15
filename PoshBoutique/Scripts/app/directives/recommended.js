poshBoutiqueApp.directive('recommended', ["articlesDataService", "$interval", function (articlesDataService, $interval) {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            changeInterval: '@'
        },
        templateUrl: 'partials/recommended.html',
        link: function (scope, element, attr) {
            scope.selectedIndex = 0;
            scope.articlesCount = 1;
            articlesDataService.getRecommendedArticles()
                .success(function (recommendedArticles) {
                    scope.articlesCount = recommendedArticles.length;
                    scope.recommendedArticles = recommendedArticles;
                });

            var intervalPromise = $interval(function () {
                if (!element.is(':hover')) {
                    scope.selectedIndex = (scope.selectedIndex + 1) % scope.articlesCount;
                }
            }, scope.changeInterval);

            scope.$on("$destroy", function () {
                if (intervalPromise) {
                    $interval.cancel(intervalPromise)
                }
            });
        }
    };
}]);