poshBoutiqueApp.controller('catalogueController', ["$scope", "categoriesTree", "articleListParams", function ($scope, categoriesTree, articleListParams) {
    console.log("1: HITTTTTTTTTTTTTTT!!!!!");

    $scope.categories = categoriesTree;
    $scope.listParams = articleListParams;

    $scope.toggleExpand = function (event, category) {
        if (category.childCategories && category.childCategories.length != 0 && event.target.tagName) {
            if (event.target.tagName == "A") {
                category.isExpanded = true;
            }
            else {
                category.isExpanded = !category.isExpanded;
                event.stopPropagation();
                event.preventDefault();
            }
        }
    };
}]);