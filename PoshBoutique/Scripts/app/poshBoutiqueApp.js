var poshBoutiqueApp = angular.module('poshBoutiqueApp', ['ui.router', 'ui.router.router', 'ui.bootstrap', 'angularTree', 'ngProgressLite', 'authenticationStorage']);

poshBoutiqueApp

    .provider('articleListParams', function () {
        var filterDefaults = { text: null };
        var orderDefaults = {
            options: [],
            by: null,
            asc: true
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

    .config(function ($stateProvider, $urlRouterProvider, articleListParamsProvider, $httpProvider) {

        $urlRouterProvider.otherwise('/');
        $urlRouterProvider.when(/(.*)\/view\/(.*)/i, ['$match', '$stateParams', '$state', 'detailsViewParentState', 'singleProductModal',
            function ($match, $stateParams, $state, detailsViewParentState, singleProductModal) {
                var stateData = detailsViewParentState.getStateData($match);
                if (!stateData) {
                    return false;
                }

                if ($state.$current.name != stateData.stateName || !equalForKeys(stateData.stateParams, $stateParams)) {
                    $state.transitionTo(stateData.stateName, stateData.stateParams, { location: false });
                }

                singleProductModal.open(stateData.itemUrl);

                return true;
            }]);

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
                  url: "/*categoryUrl",
                  templateUrl: "partials/productsListPlaceholder.html",
                  controller: function ($scope, $stateParams, listData) {
                      console.log("2: HITTTTTTTTTTTTTTT!!!!!");
                      $scope.listData = listData;
                  },
                  resolve: {
                      listData: function (articlesDataService, $stateParams, articleListParams) {
                          return articlesDataService.getArticlesInCategory(
                              $stateParams.categoryUrl,
                              articleListParams.filter.text,
                              articleListParams.order.by.value,
                              articleListParams.order.asc ? 1 : 2);
                      }
                  }
              })
            .state('autherror', {
                url: "/autherror",
                templateUrl: "partials/autherror.html",
                controller: 'autherrorController'
            });
        //.state('catalogue.category.viewItem', {
        //    url: "/view/:itemUrl",
        //    onEnter: function ($stateParams, singleProductModal) {
        //        singleProductModal.open($stateParams.itemUrl);
        //    }
        //});

        var order = {
            options: [
                { title: "Дата", value: "date" },
                { title: "Име", value: "title" },
                { title: "Цена", value: "price" },
                { title: "Харесвания", value: "likes" },
                { title: "Поръчвания", value: "orders" }
            ]
        };
        order.by = order.options[0];

        articleListParamsProvider.setOrderDefaults(order);

        $httpProvider.interceptors.push(function ($q, $window, authenticationStorage) {
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
                    return response || $q.when(response);
                },
                'responseError': function (rejection) {
                    return $q.reject(rejection);
                }
            };
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
  '$rootScope', '$modalStack', 'ngProgressLite', 'currentUser',
    function ($rootScope, $modalStack, ngProgressLite, currentUser) {
        currentUser.loadData();

        $rootScope.$on('$locationChangeStart', function () {
            var top = $modalStack.getTop();
            if (top) {
                $modalStack.dismiss(top.key);
            }
        });

        $rootScope.$on('$stateChangeStart', function () {
            ngProgressLite.start();
        });

        $rootScope.$on('$stateChangeSuccess', function () {
            ngProgressLite.done();
        });

        $rootScope.$on('$stateChangeError', function () {
            ngProgressLite.done();
        });
    }]);