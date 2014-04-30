poshBoutiqueApp.factory("currentUser", function (authenticationStorage, accountDataService) {
    var user = { };

    var setDefaults = function () {
        user.isAuthenticated = false;
        user.firstName = null;
        user.lastName = null;
    };

    
    user.loadData = function () {
        var accessToken = authenticationStorage.getAccesToken();
        if (accessToken) {
            user.isAuthenticated = true;
            accountDataService.getUserInfo()
                .success(function (data) {
                    user.firstName = data.firstName;
                    user.lastName = data.lastName;
                })
                .error(function () {
                    setDefaults();
                });
        }
        else {
            setDefaults();
        }
    }

    setDefaults();

    return user;
});