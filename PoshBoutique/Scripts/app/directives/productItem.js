poshBoutiqueApp.directive('productItem', function () {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            item: '=',
            categoryUrl: '@'
        },
        templateUrl: 'partials/productItem.html',
        controller: function ($scope, likesDataService, currentUser, authenticateModal) {
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