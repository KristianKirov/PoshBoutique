var poshBoutiqueApp = angular.module('poshBoutiqueApp', ['ui.router', 'ui.router.router', 'ui.bootstrap', 'angularTree', 'ngProgressLite']);

poshBoutiqueApp

    .provider('articleListParams', function () {
        var filterDefaults = { text: null };
        var orderDefaults = {
            options: [],
            by: null,
            asc: false
        };

        this.setFilterDefaults = function (filter) {
            angular.extend(filterDefaults, filter)
        };

        this.setOrderDefaults = function (order) {
            angular.extend(orderDefaults, order)
        };

        this.$get = function () {
            return {
                filter: filterDefaults,
                order: orderDefaults
            };
        };
    })

    .config(function ($stateProvider, $urlRouterProvider, articleListParamsProvider, $httpProvider, $provide) {

        $urlRouterProvider.otherwise('/');
        //$urlRouterProvider.when(/(.*)\/view\/(.*)/i, ['$match', '$stateParams', '$state', 'articleUrlProvider', 'singleProductModal', '$location',
        //    function ($match, $stateParams, $state, articleUrlProvider, singleProductModal, $location) {
        //        debugger;
        //        var urlData = articleUrlProvider.getUrlSegments();
        //        if (!urlData.itemUrl) {
        //            return false;
        //        }

        //        //var stateData = detailsViewParentState.getStateData($match);
        //        //if (!stateData) {
        //        //    return false;
        //        //}

        //        //if ($state.$current.name != stateData.stateName || !equalForKeys(stateData.stateParams, $stateParams)) {
        //        //    $state.transitionTo(stateData.stateName, stateData.stateParams, { location: false });
        //        //}

        //        singleProductModal.open(urlData.itemUrl);

        //        return true;
        //    }]);

        $stateProvider
          .state('home', {
              url: "/",
              templateUrl: "partials/home.html",
              controller: 'homeController'
          })
          .state('catalogue', {
              abstract: true,
              url: "/catalogue",
              templateUrl: "partials/catalogue.html",
              controller: 'catalogueController',
              resolve: {
                  categoriesTree: function (categoriesDataService) {
                      return categoriesDataService.getTree();
                  }
              }
          })
              .state('catalogue.category', {
                  url: "/:categoryUrl",
                  templateUrl: "partials/productsListPlaceholder.html",
                  controller: function ($scope, listData, categoriesDataService, articleListParams) {
                      console.log("2: HITTTTTTTTTTTTTTT!!!!!");
                      $scope.articleListParams = articleListParams;
                      $scope.listData = listData;
                      categoriesDataService.setSelectedCategory(listData.category.id);
                  },
                  resolve: {
                      listData: function (articlesDataService, $stateParams/*, articleListParams, articleUrlProvider, singleProductModal*/) {
                          //return articlesDataService.getArticlesInCategory(
                          //    $stateParams.categoryUrl,
                          //    articleListParams.filter.text,
                          //    articleListParams.order.by.value,
                          //    articleListParams.order.asc ? 1 : 2);
                          //var urlData = articleUrlProvider.getUrlSegments($stateParams.categoryUrl);

                          //if (urlData.itemUrl) {
                          //    singleProductModal.open(urlData.itemUrl);
                          //}

                          return articlesDataService.getArticlesInCategory(
                              $stateParams.categoryUrl,
                              "",
                              "",
                              1);
                      }
                  }
              })
                .state('catalogue.category.view', {
                    url: "/view/:itemUrl",
                    onEnter: function ($stateParams, singleProductModal) {
                        console.log("3: HITTTTTTTTTTTTTTT!!!!!");
                        singleProductModal.open($stateParams.itemUrl);
                    },
                    data: {
                        isDetailsState: true
                    }
                })
        .state('autherror', {
            url: "/autherror",
            templateUrl: "partials/autherror.html",
            controller: 'autherrorController'
        })
        .state('login', {
            url: "/login?returnUrl",
            controller: function ($stateParams, authenticateModal) {
                authenticateModal.open($stateParams.returnUrl);
            }
        })
        .state('cart', {
            abstract: true,
            url: "/cart",
            templateUrl: "partials/cart/cart.html",
            controller: 'cartController'
        })
            .state('cart.order', {
                url: "/order",
                templateUrl: "partials/cart/order.html",
                controller: 'cartOrderController'
            })
            .state('cart.address', {
                url: "/address",
                templateUrl: "partials/cart/address.html",
                controller: 'cartAddressController',
                data: {
                    authenticated: true
                }
            })
            .state('cart.payment', {
                url: "/payment",
                templateUrl: "partials/cart/payment.html",
                controller: 'cartPaymentController',
                data: {
                    authenticated: true
                }
            })
            .state('cart.confirmation', {
                url: "/confirmation",
                templateUrl: "partials/cart/confirmation.html",
                controller: 'cartConfirmationController',
                data: {
                    authenticated: true
                }
            })
        .state('contact-us', {
            url: "/contact-us",
            templateUrl: "partials/contactUs.html",
            controller: 'contactUsController'
        })
        .state('loyal-customer', {
            url: "/loyal-customer",
            templateUrl: "partials/loyalCustomer.html"
        })
        .state('new', {
            url: "/new",
            templateUrl: "partials/new.html",
            controller: function ($scope, articlesDataService) {
                $scope.onCollectionSelected = function (collection) {
                    articlesDataService.getArticlesInCollection(collection.id)
                        .success(function (articlesInCollection) {
                            $scope.productsInCollection = articlesInCollection;
                        })
                    .error(function () {
                        $scope.productsInCollection = null;
                    });
                }
            }
        })
            .state('new.view', {
                url: "/view/:itemUrl",
                onEnter: function ($stateParams, singleProductModal) {
                    console.log("3: HITTTTTTTTTTTTTTT!!!!!");
                    singleProductModal.open($stateParams.itemUrl);
                },
                data: {
                    isDetailsState: true
                }
            })
        .state('discount', {
            url: "/discount",
            templateUrl: "partials/discount.html",
            resolve: {
                discountedArticles: function (articlesDataService) {
                    return articlesDataService.getDiscountedArticles();
                }
            },
            controller: function ($scope, discountedArticles) {
                $scope.articles = discountedArticles;
            }
        })
            .state('discount.view', {
                url: "/view/:itemUrl",
                onEnter: function ($stateParams, singleProductModal) {
                    console.log("3: HITTTTTTTTTTTTTTT!!!!!");
                    singleProductModal.open($stateParams.itemUrl);
                },
                data: {
                    isDetailsState: true
                }
            })
        .state('featured', {
            url: "/featured",
            templateUrl: "partials/featured.html",
            resolve: {
                featuredArticles: function (articlesDataService) {
                    return articlesDataService.getFeaturedArticles();
                }
            },
            controller: function ($scope, featuredArticles) {
                $scope.articles = featuredArticles;
            }
        })
            .state('featured.view', {
                url: "/view/:itemUrl",
                onEnter: function ($stateParams, singleProductModal) {
                    console.log("3: HITTTTTTTTTTTTTTT!!!!!");
                    singleProductModal.open($stateParams.itemUrl);
                },
                data: {
                    isDetailsState: true
                }
            })
        .state('favourites', {
            url: "/favourites",
            templateUrl: "partials/favourites.html",
            resolve: {
                likedArticles: function (articlesDataService) {
                    return articlesDataService.getLikedArticles();
                }
            },
            controller: function ($scope, likedArticles, likesDataService) {
                $scope.likedArticles = likedArticles;

                $scope.articleUnlikedCallback = function (articleIndex, unlikedArticle) {
                    $scope.likedArticles.splice(articleIndex, 1);
                };
            },
            data: {
                authenticated: true
            }
        })
        .state('search', {
            url: "/search?term",
            templateUrl: "partials/search.html",
            resolve: {
                foundArticles: function (articlesDataService, $stateParams) {
                    return articlesDataService.findArticles($stateParams.term);
                }
            },
            controller: function ($scope, $stateParams, foundArticles) {
                $scope.searchTerm = $stateParams.term;
                $scope.foundArticles = foundArticles;
            }
        })
        .state('authenticateExternalLogin', {
            url: "/access_token*tokenParams",
            template: "",
            controller: function ($stateParams, authenticationStorage, $window) {
                debugger;
                var token = parseQueryString('access_token' + $stateParams.tokenParams)["access_token"];
                if (token) {
                    authenticationStorage.setAccesToken(token, false);
                }

                var initialUrl = parseQueryString($window.location.search.substr(1))["ru"];
                if (initialUrl) {
                    $window.location.href = initialUrl;
                }
            }
        });

        var order = {
            options: [
                { title: "Дата", value: "dateCreated" },
                { title: "Име", value: "title" },
                { title: "Цена", value: "price" },
                { title: "Харесвания", value: "likesCount" },
                { title: "Поръчвания", value: "ordersCount" }
            ]
        };
        order.by = order.options[0];

        articleListParamsProvider.setOrderDefaults(order);

        $httpProvider.interceptors.push(function ($q, $window, authenticationStorage, $injector) {
            function onUnauthorized() {
                var returnUrl = $window.location.href;

                var $state = $injector.get("$state");
                $state.transitionTo("login", { returnUrl: returnUrl })
            };

            return {
                'request': function (config) {
                    var accessToken = authenticationStorage.getAccesToken();
                    if (accessToken) {
                        config.headers = config.headers || {};

                        config.headers.Authorization = 'Bearer ' + accessToken;
                    }

                    return config || $q.when(config);
                },
                'requestError': function (rejection) {
                    return $q.reject(rejection);
                },
                'response': function (response) {
                    if (response.status === 401) {
                        onUnauthorized();
                    }

                    return response || $q.when(response);
                },
                'responseError': function (rejection) {
                    if (rejection.status === 401) {
                        onUnauthorized();
                    }

                    return $q.reject(rejection);
                }
            };
        });

        $provide.decorator('$state', function ($delegate, $stateParams) {
            $delegate.forceReload = function () {
                return $delegate.transitionTo($delegate.current, $stateParams, {
                    reload: true,
                    inherit: false,
                    notify: true
                });
            };
            return $delegate;
        });

        $httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
        // Override $http service's default transformRequest
        $httpProvider.defaults.transformRequest = [function (data) {
            /**
             * The workhorse; converts an object to x-www-form-urlencoded serialization.
             * @param {Object} obj
             * @return {String}
             */
            var param = function (obj) {
                var query = '';
                var name, value, fullSubName, subName, subValue, innerObj, i;
                for (name in obj) {
                    value = obj[name];
                    if (value instanceof Array) {
                        for (i = 0; i < value.length; ++i) {
                            subValue = value[i];
                            fullSubName = name + '[' + i + ']';
                            innerObj = {};
                            innerObj[fullSubName] = subValue;
                            query += param(innerObj) + '&';
                        }
                    }
                    else if (value instanceof Object) {
                        for (subName in value) {
                            subValue = value[subName];
                            fullSubName = name + '[' + subName + ']';
                            innerObj = {};
                            innerObj[fullSubName] = subValue;
                            query += param(innerObj) + '&';
                        }
                    }
                    else if (value !== undefined && value !== null) {
                        query += encodeURIComponent(name) + '=' + encodeURIComponent(value) + '&';
                    }
                }
                return query.length ? query.substr(0, query.length - 1) : query;
            };
            return angular.isObject(data) && String(data) !== '[object File]' ? param(data) : data;
        }];
    }).run([
  '$rootScope', '$modalStack', 'ngProgressLite', 'currentUser', 'authenticateModal', '$window',
    function ($rootScope, $modalStack, ngProgressLite, currentUser, authenticateModal, $window) {
        debugger;
        currentUser.loadData();

        $rootScope.$on('$locationChangeStart', function () {
            var top = $modalStack.getTop();
            if (top) {
                $modalStack.dismiss(top.key);
            }
        });

        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            if (toState.data) {
                var authenticate = toState.data.authenticated;
                if (authenticate && !currentUser.isAuthenticated) {
                    debugger;
                    event.preventDefault();

                    var returnUrl = null;
                    var toNav = toState.navigable;
                    if (toNav) {
                        returnUrl = toNav.url.format(toParams);
                    }
                    else {
                        returnUrl = $window.location.href;
                    }
                    authenticateModal.open(returnUrl);
                    return;
                }
            }
            
            ngProgressLite.start();
        });

        $rootScope.$on('$stateChangeSuccess', function () {
            ngProgressLite.done();
        });

        $rootScope.$on('$stateChangeError', function () {
            ngProgressLite.done();
        });
    }]);