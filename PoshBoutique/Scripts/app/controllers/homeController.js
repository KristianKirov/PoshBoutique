poshBoutiqueApp.controller('homeController', function ($scope, authenticateModal) {
    $scope.data = "works";

    authenticateModal.open();
});