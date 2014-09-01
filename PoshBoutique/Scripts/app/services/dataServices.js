poshBoutiqueApp.factory("categoriesDataService", function ($http) {
    return {
        getTree: function () {
            return $http({ method: 'GET', url: '/api/categories/tree' })
                .then(function (response) {
                    var categoriesTree = response.data;
                    //for (category in categoriesTree) {
                    //    if (category.childCategories && childCategories.length > 0) {
                    //        category.isExpanded = true;
                    //    }
                    //}

                    return categoriesTree;
                });
        }
    };
});

poshBoutiqueApp.factory("articlesDataService", function ($http) {
    return {
        getArticlesInCategory: function (categoryUrl, filter, orderBy, sortDirection) {
            return $http({
                method: 'GET',
                url: '/api/articles',
                params: {
                    categoryUrl: categoryUrl,
                    filter: filter,
                    orderBy: orderBy,
                    sortDirection: sortDirection
                }
            }).then(function (response) {
                    var listData = response.data;

                    return listData;
                });
        },
        getArticleByUrlName: function(articleUrlName) {
            return $http({
                method: 'GET',
                url: '/api/articles',
                params: {
                    urlName: articleUrlName
                }
            }).then(function (response) {
                var listData = response.data;

                return listData;
            });
        }
    };
});