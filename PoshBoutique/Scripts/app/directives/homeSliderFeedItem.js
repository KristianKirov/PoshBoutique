poshBoutiqueApp.directive('homeSliderFeedItem', function ($timeout) {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            displayOptions: '=',
            visible: '='
        },
        templateUrl: 'partials/homeSliderFeedItem.html',
        link: function (scope, element, attrs) {
            var showPromise = null;
            var hidePromise = null;

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
            var startShowPromise = function () {
                showPromise = $timeout(function () {
                    showFeedItem();
                }, scope.displayOptions.showAfter);
            };

            var startHidePromise = function () {
                hidePromise = $timeout(function () {
                    hideFeedItem();
                }, scope.displayOptions.changeInterval - scope.displayOptions.hideBefore);
            }

            var stopTimeouts = function () {
                if (showPromise) {
                    $timeout.cancel(showPromise)
                }

                if (hidePromise) {
                    $timeout.cancel(hidePromise)
                }
            };

            startShowPromise();
            startHidePromise();

            scope.$watch('visible', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    stopTimeouts();

                    if (newValue) {
                        startShowPromise();
                        startHidePromise();
                    }
                    else {
                        hideFeedItem();
                    }
                }
            });
        }
    };
});