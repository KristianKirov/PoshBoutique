poshBoutiqueApp.factory("singleProductModal", function ($modal, $state, $stateParams, $location) {
    return {
        open: function (itemUrlName) {
            var setPreviousUrl = function () {
                var currentLocation = $state.href($state.current, $stateParams).substring(1);
                if ($location.url() !== currentLocation) {
                    $location.url(currentLocation);
                    $location.replace();
                }
            };

            $modal.open({
                templateUrl: "partials/singleProductModal.html",
                resolve: {
                    product: function (articlesDataService) {
                        return articlesDataService.getArticleByUrlName(itemUrlName);
                    }
                },
                controller: ['$scope', 'product', function ($scope, product) {
                    $scope.product = product;

                    $scope.dismiss = function () {
                        $scope.$dismiss();
                    };

                    $scope.close = function () {
                        $scope.$close(true);
                    };
                }]
            }).result.then(function (result) {
                setPreviousUrl();
            },
            function () {
                setPreviousUrl();
            });
        }
    };
});