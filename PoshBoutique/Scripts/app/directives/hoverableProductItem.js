poshBoutiqueApp.directive('hoverableProductItem', function () {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            item: '=',
            showLikeButton: '@',
            detailsSref: '@'
        },
        templateUrl: 'partials/hoverableProductItem.html',
        controller: function ($scope, likesDataService, currentUser, authenticateModal, $rootScope) {
            if (!$scope.detailsSref) {
                $scope.detailsSref = '.view({ itemUrl: item.urlName })';
            }

            $scope.toggleLike = function (item, e) {
                e.preventDefault();
                e.stopPropagation();
                if (currentUser.isAuthenticated) {
                    var likeFunction = item.isLiked ? likesDataService.unlikeArticle : likesDataService.likeArticle;
                    likeFunction(item.id);
                }
                else {
                    authenticateModal.open();
                }
            };

            $rootScope.$on('toggleLike', function (event, articleId, isLiked) {
                if (articleId == $scope.item.id) {
                    $scope.item.isLiked = isLiked;
                }
            });
        }
    };
});