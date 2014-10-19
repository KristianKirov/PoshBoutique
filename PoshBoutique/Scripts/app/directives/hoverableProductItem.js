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
        controller: function ($scope, likesDataService, currentUser, authenticateModal) {
            if (!$scope.detailsSref) {
                $scope.detailsSref = '.view({ itemUrl: item.urlName })';
            }

            $scope.toggleLike = function (item, e) {
                e.preventDefault();
                e.stopPropagation();
                if (currentUser.isAuthenticated) {
                    var likeFunction = item.isLiked ? likesDataService.unlikeArticle : likesDataService.likeArticle;
                    likeFunction(item.id)
                        .success(function () {
                            item.isLiked = !item.isLiked;
                        });
                }
                else {
                    authenticateModal.open();
                }
            };
        }
    };
});