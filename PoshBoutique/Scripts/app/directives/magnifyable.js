poshBoutiqueApp.directive('magnifyable', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var magnifiedWidth = attrs.mWidth;
            var magnifiedHeight = attrs.mHeight;
            var magnifier = element.children(".large-image-view");
            var magnifierWidth = magnifier.width();
            var magnifierHeight = magnifier.height();
            var magnifierCenterX = magnifierWidth / 2;
            var magnifierCenterY = magnifierHeight / 2;
            var backgroundMinX = magnifierWidth - magnifiedWidth;
            var backgroundMinY = magnifierHeight - magnifiedHeight;

            magnifier.on("click", function () {
                if (magnifier.is(":visible")) {
                    magnifier.hide();
                }
            });

            element.mousemove(function (e) {
                var magnifyableWidth = element.width();
                var magnifyableHeight = element.height();
                var magnifyableOffset = element.offset();


                var mouseX = e.pageX - magnifyableOffset.left;
                var mouseY = e.pageY - magnifyableOffset.top;

                var magnifyXFactor = magnifiedWidth / magnifyableWidth;
                var magnifyYFactor = magnifiedHeight / magnifyableHeight;

                var magnifiedX = mouseX * magnifyXFactor;
                var magnifiedY = mouseY * magnifyYFactor;

                var backgroundX = magnifierCenterX - magnifiedX;
                var backgroundY = magnifierCenterY - magnifiedY;

                var magnifierX = mouseX - magnifierCenterX;
                var magnifierY = mouseY - magnifierCenterY;

                if (backgroundX > 0) backgroundX = 0;
                if (backgroundY > 0) backgroundY = 0;
                if (backgroundX < backgroundMinX) backgroundX = backgroundMinX;
                if (backgroundY < backgroundMinY) backgroundY = backgroundMinY;
                
                if (mouseX < 0 || mouseY < 0 || mouseX > magnifyableWidth || mouseY > magnifyableHeight || $(e.target).hasClass("hide-magnfier")) {
                    if (magnifier.is(":visible")) {
                        magnifier.fadeOut(500);
                    }
                }
                else {
                    if (!magnifier.is(":visible")) {
                        magnifier.fadeIn(500);
                    }

                    magnifier.css({ top: magnifierY + "px" , left: magnifierX + "px", backgroundPosition: backgroundX + "px " + backgroundY + "px" });
                }
            });
        }
    }
});