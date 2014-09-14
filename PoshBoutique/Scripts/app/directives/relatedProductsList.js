poshBoutiqueApp.directive('relatedProductsList', function () {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            articleId: '@',
            onItemSelected: '&'
        },
        templateUrl: 'partials/relatedProductsList.html',
        controller: function ($scope, articlesDataService) {
            $scope.hasRelatedArticles = false;
            articlesDataService.getRelatedArticles($scope.articleId)
                .success(function (data) {
                    $scope.hasRelatedArticles = true;
                    $scope.relatedArticles = data;
                })
                .error(function () {
                    $scope.hasRelatedArticles = false;
                });

            $scope.invokeOnItemSelected = function () {
                $scope.onItemSelected();
            };
        }
    };
});