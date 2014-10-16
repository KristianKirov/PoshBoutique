poshBoutiqueApp.directive('rowProductItem', function () {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            item: '=',
            showDislikeButton: '@',
            onArticleUnliked: '&'
        },
        templateUrl: 'partials/rowProductItem.html',
        controller: function ($scope, likesDataService) {
            $scope.unlikeArticle = function (unlikedArticle) {
                $scope.onArticleUnliked({ unlikedArticle: unlikedArticle });

                likesDataService.unlikeArticle(unlikedArticle.id);
            };
        }
    };
});