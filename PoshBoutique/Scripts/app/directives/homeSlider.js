poshBoutiqueApp.directive('homeSlider', function () {
    return {
        restrict: 'E',
        replace: false,
        scope: {
            changeInterval: '@'
        },
        templateUrl: 'partials/homeSlider.html',
        controller: function ($scope) {
            var placeholderElements = angular.element("html");
            placeholderElements.addClass("fullPage");
            $scope.$on("$destroy", function () {
                placeholderElements.removeClass("fullPage");
            });

            $scope.slides = [
                {
                    image: "http://lorempixel.com/1700/900/fashion/?q=1",
                    feedItems: [{
                        color: "white",
                        width: 150,
                        showAfter: 1000,
                        hideBefore: 1000,
                        right: 10,
                        top: 20,
                        changeInterval: $scope.changeInterval,
                        title: "Коледна колекция",
                        description: "Аасоидй оаисдйои ешииш ошоиеи дис осоид мос оиосиеоиф е."
                    },
                    {
                        color: "red",
                        width: 300,
                        showAfter: 1500,
                        hideBefore: 1500,
                        left: 40,
                        bottom: 15,
                        changeInterval: $scope.changeInterval,
                        title: "POSH boutique"
                    }]
                },
                {
                    image: "http://lorempixel.com/1700/900/fashion/?q=2",
                    feedItems: [{
                        color: "green",
                        width: 300,
                        showAfter: 1000,
                        hideBefore: 1000,
                        left: 40,
                        top: 15,
                        changeInterval: $scope.changeInterval,
                        title: "Горе",
                        description: "Кйс кйкй хр осиеху ру ее иеухеуи шиеу."
                    },
                    {
                        color: "blue",
                        width: 300,
                        showAfter: 1500,
                        hideBefore: 1500,
                        left: 10,
                        top: 30,
                        changeInterval: $scope.changeInterval,
                        title: "Ляво"
                    }]
                }
            ];
        }
    };
});