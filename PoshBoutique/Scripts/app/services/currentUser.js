poshBoutiqueApp.factory("currentUser", function (authenticationStorage, accountDataService) {
    var user = {};

    var setDefaults = function (isAuthenticated) {
        user.isAuthenticated = isAuthenticated;
        user.firstName = null;
        user.lastName = null;
        user.email = null;
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
                    user.email = data.email;
                })
                .error(function () {
                    setDefaults(false);
                });
        }
        else {
            setDefaults(false);
        }
    };

    user.logout = function () {
        authenticationStorage.removeAccesToken();
        setDefaults(false);
    };

    user.login = function (accessToken, persistent) {
        authenticationStorage.setAccesToken(accessToken, persistent);
        user.loadData();
    };

    return user;
});