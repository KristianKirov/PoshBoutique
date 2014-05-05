poshBoutiqueApp.factory("currentUser", function (authenticationStorage, accountDataService) {
    var user = {};

    var setDefaults = function (isAuthenticated) {
        user.isAuthenticated = isAuthenticated;
        user.firstName = null;
        user.lastName = null;
    };

    setDefaults(!!authenticationStorage.getAccesToken());

    user.loadData = function () {
        var accessToken = authenticationStorage.getAccesToken();
        if (accessToken) {
            user.isAuthenticated = true;
            accountDataService.getUserInfo()
                .success(function (data) {
                    user.isAuthenticated = true;
                    user.firstName = data.firstName;
                    user.lastName = data.lastName;
                })
                .error(function () {
                    setDefaults(false);
                });
        }
        else {
            setDefaults(false);
        }
    }

    return user;
});