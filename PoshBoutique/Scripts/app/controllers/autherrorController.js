poshBoutiqueApp.controller('autherrorController', function ($scope, $window) {
    var queryString = parseQueryString($window.location.search.substr(1));
    $scope.code = queryString.code;
    $scope.data = queryString.data;
});