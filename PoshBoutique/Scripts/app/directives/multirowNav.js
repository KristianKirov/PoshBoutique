poshBoutiqueApp.directive('multirowNav', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            var $body = $("body");
            var currentPaddingTop = $body.css("padding-top");

            var navHeight = element.height() + "px";
            if (navHeight != currentPaddingTop) {
                $body.css("padding-top", navHeight);
            }

            scope.$watch(function () {
                return element.height();
            },
            function (newValue, oldValue) {
                if (newValue != oldValue) {
                    $body.css("padding-top", newValue + "px");
                }
            });
        }
    };
});