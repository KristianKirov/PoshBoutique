poshBoutiqueApp.factory("authenticationStorage", ["$window", function ($window) {
    return {
        setAccesToken: function (accessToken, persistent) {
            if (persistent) {
                $window.localStorage["accessToken"] = accessToken;
            } else {
                $window.sessionStorage["accessToken"] = accessToken;
            }
        },
        getAccesToken: function () {
            var accessToken = $window.sessionStorage["accessToken"] || $window.localStorage["accessToken"];

            return accessToken;
        },
        removeAccesToken: function () {
            $window.localStorage.removeItem("accessToken");
            $window.sessionStorage.removeItem("accessToken");
        }
    }
}]);