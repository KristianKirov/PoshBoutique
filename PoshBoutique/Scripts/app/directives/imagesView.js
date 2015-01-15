poshBoutiqueApp.directive('imagesView', function () {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            product: '='
        },
        templateUrl: 'partials/imagesView.html',
        controller: ["$scope", "$modal", "likesDataService", "currentUser", "authenticateModal", "$rootScope", function ($scope, $modal, likesDataService, currentUser, authenticateModal, $rootScope) {
            $scope.images = $scope.product.images;
            $scope.selectedImage = $scope.images[0];

            $scope.isImageSelected = function (image) {
                return $scope.selectedImage == image;
            };

            $scope.selectPrevImage = function () {
                var selectedImageIndex = $.inArray($scope.selectedImage, $scope.images);
                var prevImageIndex = (selectedImageIndex > 0) ? --selectedImageIndex : $scope.images.length - 1;
                $scope.selectedImage = $scope.images[prevImageIndex];

                $scope.scrollToImageWithIndex(prevImageIndex);
            };

            $scope.selectNextImage = function () {
                var selectedImageIndex = $.inArray($scope.selectedImage, $scope.images);
                var nextImageIndex = (selectedImageIndex < $scope.images.length - 1) ? ++selectedImageIndex : 0;
                $scope.selectedImage = $scope.images[nextImageIndex];

                $scope.scrollToImageWithIndex(nextImageIndex);
            };

            $scope.selectImage = function (image) {
                $scope.selectedImage = image;
            };

            $scope.openSelectedImage = function () {
                $modal.open({
                    windowClass: "images-modal",
                    template: "<button type='button' class='close' ng-click='$close()'>&times;</button><div class='arrow arrow--left' ng-click='selectPrevImage()'><i class='glyphicon glyphicon-chevron-left'></i></div><div class='arrow arrow--right' ng-click='selectNextImage()'><i class='glyphicon glyphicon-chevron-right'></i></div> <img ng-src='{{selectedImage.largeUrl}}' />",
                    scope: $scope
                });
            };

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
                debugger;
                if (articleId == $scope.product.id) {
                    $scope.product.isLiked = isLiked;
                }
            });
        }],
        link: function (scope, element, attrs) {
            var $imagesListWrapper = element.find(".images-view-picker ul")
            var imagesListWrapper = $imagesListWrapper[0];

            var setScrollLeft = function (scrollLeftForElement) {
                var maxScrollLeft = imagesListWrapper.scrollWidth - imagesListWrapper.clientWidth;

                scope.showLeftArrow = scrollLeftForElement > 0;
                scope.showRightArrow = scrollLeftForElement < maxScrollLeft;

                if (!scope.showLeftArrow) {
                    scrollLeftForElement = 0;
                }

                if (!scope.showRightArrow) {
                    scrollLeftForElement = maxScrollLeft;
                }

                $imagesListWrapper.animate({ scrollLeft: scrollLeftForElement }, { duration: '100', easing: 'swing' });
            };

            scope.scrollToImageWithIndex = function (imageIndex) {
                var elementWidth = imagesListWrapper.scrollWidth / imagesListWrapper.children.length;
                var scrollLeftForElement = ((imageIndex + 0.5) * elementWidth) - (imagesListWrapper.clientWidth / 2);

                setScrollLeft(scrollLeftForElement);
            };

            scope.scrollPickerLeft = function () {
                var elementWidth = imagesListWrapper.scrollWidth / imagesListWrapper.children.length;

                var scrollLeftForElement = imagesListWrapper.scrollLeft - elementWidth;

                setScrollLeft(scrollLeftForElement);
            };

            scope.scrollPickerRight = function () {
                var elementWidth = imagesListWrapper.scrollWidth / imagesListWrapper.children.length;

                var scrollLeftForElement = imagesListWrapper.scrollLeft + elementWidth;

                setScrollLeft(scrollLeftForElement);
            };

            scope.$watch(function () {
                return imagesListWrapper.scrollWidth - imagesListWrapper.clientWidth;
            }, function (oldVal, newVal) {
                if (imagesListWrapper.scrollWidth > imagesListWrapper.clientWidth) {
                    scope.showLeftArrow = imagesListWrapper.scrollLeft > 0;
                    scope.showRightArrow = imagesListWrapper.scrollLeft < (imagesListWrapper.scrollWidth - imagesListWrapper.clientWidth);
                }
            });
        }
    };
});