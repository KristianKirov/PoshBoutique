poshBoutiqueApp.directive('homeSliderFeedItem', function ($timeout) {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            displayOptions: '='
        },
        templateUrl: 'partials/homeSliderFeedItem.html',
        link: function (scope, element, attrs) {
            var verticalValue = null;
            var verticalName = null;
            var horizontalValue = null;
            var horizontalName = null;

            if (scope.displayOptions.top) {
                verticalName = "top";
                verticalValue = scope.displayOptions.top;
            }
            else {
                verticalName = "bottom";
                verticalValue = scope.displayOptions.bottom;
            }

            if (scope.displayOptions.left) {
                horizontalName = "left";
                horizontalValue = scope.displayOptions.left;
            }
            else {
                horizontalName = "right";
                horizontalValue = scope.displayOptions.right;
            }

            element.css({ "color": scope.displayOptions.color, "width": scope.displayOptions.width + "px" });

            var hideFeedItem = function () {
                var cssOptions = { "opacity": 0 };
                if (verticalValue < horizontalValue) {
                    cssOptions[verticalName] = 0;
                    cssOptions[horizontalName] = horizontalValue + "%";
                }
                else {
                    cssOptions[verticalName] = verticalValue + "%";
                    cssOptions[horizontalName] = 0;
                }

                element.css(cssOptions);
            };

            var showFeedItem = function () {
                var cssOptions = { "opacity": 1 };
                if (verticalValue < horizontalValue) {
                    cssOptions[verticalName] = verticalValue + "%";
                }
                else {
                    cssOptions[horizontalName] = horizontalValue + "%";
                }

                element.css(cssOptions);
            };

            hideFeedItem();

            $timeout(function () {
                showFeedItem();
            }, scope.displayOptions.showAfter);

            $timeout(function () {
                hideFeedItem();
            }, scope.displayOptions.changeInterval - scope.displayOptions.hideBefore);
        }
    };
});