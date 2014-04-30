poshBoutiqueApp.factory("singleProductModal", function ($modal, $state, $stateParams) {
    return {
        open: function (itemUrlName) {
            $modal.open({
                templateUrl: "partials/singleProductModal.html",
                resolve: {
                    //item: function () { new Item(123).get(); } //TODO get item from itemUrlName
                },
                controller: ['$scope'/*, 'item'*/, function ($scope/*, item*/) {
                    $scope.product = {
                        title: "Черен дизайнерски гащеризон с мрежа и кожени акценти",
                        price: "109.00000",
                        description: "Гащеризон от памук в цвят Beige с долна част тип брич, джобове и ревер. Леко еластична материя. Свободен силует в ханша.",
                        materialDescription: "текстил, еластан, мрежа, кожа",
                        sizes: [
                            { id: 1, name: "XS", colors: [ { id: 1, title: "Черен", quantity: 5 } ] },
                            { id: 2, name: "S", colors: [{ id: 1, title: "Черен", quantity: 4 }, { id: 2, title: "Син", quantity: 0 }] },
                            { id: 3, name: "M", colors: [{ id: 1, title: "Черен" }, { id: 2, title: "Син" }, { id: 3, title: "Зелен" }] },
                            { id: 4, name: "L", quantity: 10 },
                            { id: 5, name: "XL" }
                        ],
                        images: [
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img801/3451/ejc3.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img703/3074/x5tn.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img841/5209/0b8b.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img132/9161/it4a.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img713/8596/yibf.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img812/510/kjgj.jpg"
                            },
                            {
                                smallUrl: "http://imageshack.com/a/img801/9522/d2fy.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img843/8256/6wzi.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img844/4985/x1nm.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img801/3451/ejc3.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img703/3074/x5tn.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img841/5209/0b8b.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img132/9161/it4a.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img713/8596/yibf.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img812/510/kjgj.jpg"
                            },
                            {
                                smallUrl: "http://imageshack.com/a/img801/9522/d2fy.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img843/8256/6wzi.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img844/4985/x1nm.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img801/3451/ejc3.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img703/3074/x5tn.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img841/5209/0b8b.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img132/9161/it4a.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img713/8596/yibf.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img812/510/kjgj.jpg"
                            },
                            {
                                smallUrl: "http://imageshack.com/a/img801/9522/d2fy.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img843/8256/6wzi.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img844/4985/x1nm.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img801/3451/ejc3.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img703/3074/x5tn.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img841/5209/0b8b.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img132/9161/it4a.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img713/8596/yibf.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img812/510/kjgj.jpg"
                            },
                            {
                                smallUrl: "http://imageshack.com/a/img801/9522/d2fy.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img843/8256/6wzi.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img844/4985/x1nm.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img801/3451/ejc3.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img703/3074/x5tn.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img841/5209/0b8b.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img132/9161/it4a.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img713/8596/yibf.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img812/510/kjgj.jpg"
                            },
                            {
                                smallUrl: "http://imageshack.com/a/img801/9522/d2fy.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img843/8256/6wzi.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img844/4985/x1nm.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img801/3451/ejc3.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img703/3074/x5tn.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img841/5209/0b8b.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img132/9161/it4a.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img713/8596/yibf.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img812/510/kjgj.jpg"
                            },
                            {
                                smallUrl: "http://imageshack.com/a/img801/9522/d2fy.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img843/8256/6wzi.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img844/4985/x1nm.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img801/3451/ejc3.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img703/3074/x5tn.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img841/5209/0b8b.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img132/9161/it4a.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img713/8596/yibf.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img812/510/kjgj.jpg"
                            },
                            {
                                smallUrl: "http://imageshack.com/a/img801/9522/d2fy.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img843/8256/6wzi.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img844/4985/x1nm.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img801/3451/ejc3.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img703/3074/x5tn.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img841/5209/0b8b.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img132/9161/it4a.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img713/8596/yibf.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img812/510/kjgj.jpg"
                            },
                            {
                                smallUrl: "http://imageshack.com/a/img801/9522/d2fy.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img843/8256/6wzi.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img844/4985/x1nm.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img801/3451/ejc3.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img703/3074/x5tn.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img841/5209/0b8b.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img132/9161/it4a.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img713/8596/yibf.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img812/510/kjgj.jpg"
                            },
                            {
                                smallUrl: "http://imageshack.com/a/img801/9522/d2fy.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img843/8256/6wzi.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img844/4985/x1nm.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img801/3451/ejc3.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img703/3074/x5tn.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img841/5209/0b8b.jpg"
                            },
                            {
                                smallUrl: "http://imagizer.imageshack.us/a/img132/9161/it4a.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img713/8596/yibf.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img812/510/kjgj.jpg"
                            },
                            {
                                smallUrl: "http://imageshack.com/a/img801/9522/d2fy.jpg",
                                mediumUrl: "http://imagizer.imageshack.us/a/img843/8256/6wzi.jpg",
                                largeUrl: "http://imagizer.imageshack.us/a/img844/4985/x1nm.jpg"
                            }]
                    };

                    $scope.dismiss = function () {
                        $scope.$dismiss();
                    };

                    $scope.close = function () {
                        //item.update().then(function () {
                        $scope.$close(true);
                        //});
                    };
                }]
            }).result.then(function (result) {
                //if (result) {
                $state.transitionTo($state.current, $stateParams, { notify: false });
                //$state.reload();
                //}
            },
            function () {
                $state.transitionTo($state.current, $stateParams, { notify: false });
            });
        }
    };
});