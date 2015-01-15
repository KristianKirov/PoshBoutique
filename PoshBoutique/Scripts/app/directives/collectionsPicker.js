poshBoutiqueApp.directive('collectionsPicker', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: 'partials/collectionsPicker.html',
        scope: {
            onCollectionSelected: '&'
        },
        controller: ["$scope", "collectionsDataService", function ($scope, collectionsDataService) {
            var selectCollection = function (collection) {
                $scope.selectedCollection = collection;
                if ($scope.onCollectionSelected) {
                    $scope.onCollectionSelected({ collection: collection });
                }
            };

            $scope.dropdownExpanded = false;

            collectionsDataService.getAll()
                .success(function (allCollections) {
                    $scope.collections = allCollections;
                    selectCollection(allCollections[0]);
                });

            $scope.selectCollection = function (collection) {
                $scope.dropdownExpanded = false;
                if ($scope.selectedCollection.id != collection.id) {
                    selectCollection(collection);
                }
            }
        }]
    }
});