poshBoutiqueApp.factory("categoriesDataService", function ($http) {
    var cachedCategoriesTree = null;
    var initialCategoryId = null;

    var selectCategory = function (categoryId, categories) {
        debugger;
        if (!categories) {
            return false;
        }

        var result = false;
        for (var categoryIndex in categories) {
            var category = categories[categoryIndex];
            if (category.id == categoryId) {
                category.isSelected = true;
                category.isExpanded = true;

                result = true;
            }
            else {
                category.isSelected = false;    
            }

            var isChildSelected = selectCategory(categoryId, category.childCategories);
            if (!category.isExpanded && isChildSelected) {
                category.isExpanded = true;
            }
        }

        return result;
    };

    return {
        setSelectedCategory: function (categoryId) {            
            if (cachedCategoriesTree) {
                initialCategorySet = true;
                selectCategory(categoryId, cachedCategoriesTree);
            }
            else {
                initialCategoryId = categoryId;
            }
        },
        getTree: function () {
            if (cachedCategoriesTree) {
                return cachedCategoriesTree;
            }

            return $http({ method: 'GET', url: '/api/categories/tree' })
                .then(function (response) {
                    var categoriesTree = response.data;
                    //for (category in categoriesTree) {
                    //    if (category.childCategories && childCategories.length > 0) {
                    //        category.isExpanded = true;
                    //    }
                    //}
                    debugger;
                    cachedCategoriesTree = categoriesTree;
                    if (initialCategoryId) {
                        selectCategory(initialCategoryId, cachedCategoriesTree);
                        initialCategoryId = null;
                    }

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
        },
        getRelatedArticles: function (articleId) {
            return $http({
                method: 'GET',
                url: '/api/relatedarticles/' + articleId
            });
        }
    };
});

poshBoutiqueApp.factory("likesDataService", function ($http) {
    return {
        likeArticle: function (articleId) {
            return $http({
                method: 'PUT',
                url: '/api/likes/' + articleId
            });
        },
        unlikeArticle: function (articleId) {
            return $http({
                method: 'DELETE',
                url: '/api/likes/' + articleId
            });
        }
    };
});

poshBoutiqueApp.factory("subscriptionsService", function ($http) {
    return {
        subscribe: function (email) {
            return $http({
                method: 'POST',
                url: '/api/subscriptions',
                data: { email: email }
            });
        }
    };
});

poshBoutiqueApp.factory("feedbackService", function ($http) {
    return {
        submit: function (feedbackData) {
            return $http({
                method: 'POST',
                url: '/api/feedback',
                data: feedbackData
            });
        }
    };
});