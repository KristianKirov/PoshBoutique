poshBoutiqueApp.factory("accountDataService", function ($http, $window) {
    var httpsUrl = "https://" + $window.location.host;

    var addExternalLoginUrl = httpsUrl + "/api/Account/AddExternalLogin",
        changePasswordUrl = httpsUrl + "/api/Account/changePassword",
        loginUrl = httpsUrl + "/Token",
        logoutUrl = httpsUrl + "/api/Account/Logout",
        registerUrl = httpsUrl + "/api/Account/Register",
        registerExternalUrl = httpsUrl + "/api/Account/RegisterExternal",
        removeLoginUrl = httpsUrl + "/api/Account/RemoveLogin",
        setPasswordUrl = httpsUrl + "/api/Account/setPassword",
        siteUrl = httpsUrl + "/",
        userInfoUrl = httpsUrl + "/api/Account/UserInfo";

    // Route operations
    function externalLoginsUrl(returnUrl, generateState) {
        return httpsUrl + "/api/Account/ExternalLogins?returnUrl=" + (encodeURIComponent("/external-login-authenticated.html?ru=" + encodeURIComponent(returnUrl))) +
            "&generateState=" + (generateState ? "true" : "false");
    }

    function manageInfoUrl(returnUrl, generateState) {
        return httpsUrl + "/api/Account/ManageInfo?returnUrl=" + (encodeURIComponent(returnUrl)) +
            "&generateState=" + (generateState ? "true" : "false");
    }

    // Other private operations
    function getSecurityHeaders() {
        var accessToken = sessionStorage["accessToken"] || localStorage["accessToken"];

        if (accessToken) {
            return { "Authorization": "Bearer " + accessToken };
        }

        return {};
    }

    return {
        
        toErrorsArray: function (data) {
            var errors = new Array(),
                items;

            if (!data || !data.message) {
                return null;
            }

            if (data.modelState) {
                for (var key in data.modelState) {
                    items = data.modelState[key];

                    if (items.length) {
                        for (var i = 0; i < items.length; i++) {
                            errors.push(items[i]);
                        }
                    }
                }
            }

            if (errors.length === 0) {
                errors.push(data.message);
            }

            return errors;
        },

        // Data
        returnUrl: siteUrl,

        // Data access operations
        addExternalLogin: function (data) {
            return $.ajax(addExternalLoginUrl, {
                type: "POST",
                data: data,
                headers: getSecurityHeaders()
            });
        },

        changePassword: function (data) {
            return $.ajax(changePasswordUrl, {
                type: "POST",
                data: data,
                headers: getSecurityHeaders()
            });
        },

        getExternalLogins: function (returnUrl, generateState) {
            return $http({ method: "GET", url: externalLoginsUrl(returnUrl, generateState) })
                .then(function (response) {
                    return response.data;
                });
        },

        getManageInfo: function (returnUrl, generateState) {
            return $.ajax(manageInfoUrl(returnUrl, generateState), {
                cache: false,
                headers: getSecurityHeaders()
            });
        },

        getUserInfo: function () {
            return $http({ method: "GET", url: userInfoUrl });
        },

        login: function (data) {
            return $http({ method: "POST", url: loginUrl, data: data });
        },

        logout: function () {
            return $.ajax(logoutUrl, {
                type: "POST",
                headers: getSecurityHeaders()
            });
        },

        register: function (data) {
            return $http({ method: "POST", url: registerUrl, data: data });
        },

        registerExternal: function (accessToken, data) {
            return $.ajax(registerExternalUrl, {
                type: "POST",
                data: data,
                headers: {
                    "Authorization": "Bearer " + accessToken
                }
            });
        },

        removeLogin: function (data) {
            return $.ajax(removeLoginUrl, {
                type: "POST",
                data: data,
                headers: getSecurityHeaders()
            });
        },

        setPassword: function (data) {
            return $.ajax(setPasswordUrl, {
                type: "POST",
                data: data,
                headers: getSecurityHeaders()
            });
        }
    };
});