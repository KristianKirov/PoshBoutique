poshBoutiqueApp.factory("singleProductModal", function ($modal, $state) {
    return {
        open: function (itemUrlName) {
            var setPreviousUrl = function () {
                //var listUrl = articleUrlProvider.getListUrl();

                //if ($location.url() !== listUrl) {
                //    $location.url(listUrl);
                //    $location.replace();
                //}

                $state.go('^')
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