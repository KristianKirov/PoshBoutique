poshBoutiqueApp.directive('productItem', function () {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            item: '='
        },
        templateUrl: 'partials/productItem.html',
        controller: function ($scope, likesDataService, currentUser, authenticateModal, $rootScope) {
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