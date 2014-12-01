poshBoutiqueApp.directive('addressForm', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: 'partials/addressForm.html',
        scope: {
            addressInfoModel: '=',
            addressForm: '='
        }/*,
        controller: function ($scope) {

        }*/
    };
});