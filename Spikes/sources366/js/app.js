'use strict';

angular.module('example366', ['ngAnimate'])
  .controller('MainCtrl', function ($scope) {

    // Set of Photos
    $scope.photos = [
        {src: 'http://farm9.staticflickr.com/8042/7918423710_e6dd168d7c_b.jpg'},
        {src: 'http://farm9.staticflickr.com/8449/7918424278_4835c85e7a_b.jpg'},
        {src: 'http://farm9.staticflickr.com/8457/7918424412_bb641455c7_b.jpg'},
        {src: 'http://farm9.staticflickr.com/8179/7918424842_c79f7e345c_b.jpg'},
        {src: 'http://farm9.staticflickr.com/8315/7918425138_b739f0df53_b.jpg'},
        {src: 'http://farm9.staticflickr.com/8461/7918425364_fe6753aa75_b.jpg'}
    ];

    // initial image index
    $scope.visibleImageIndex = 0;

    // if a current image is the same as requested image
    $scope.isActive = function (index) {
        return $scope.visibleImageIndex === index;
    };

    // show prev image
    $scope.showPrev = function () {
        $scope.visibleImageIndex = ($scope.visibleImageIndex > 0) ? --$scope.visibleImageIndex : $scope.photos.length - 1;
    };

    // show next image
    $scope.showNext = function () {
        $scope.visibleImageIndex = ($scope.visibleImageIndex < $scope.photos.length - 1) ? ++$scope.visibleImageIndex : 0;
    };

    // show a certain image
    $scope.showPhoto = function (index) {
        $scope.visibleImageIndex = index;
    };
});

