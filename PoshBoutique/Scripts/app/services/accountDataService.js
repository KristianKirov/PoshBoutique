poshBoutiqueApp.factory("accountDataService", ["$http", "$window", function ($http, $window) {
    var httpsUrl = "https://" + $window.location.host;

       //addExternalLoginUrl = httpsUrl + "/api/account/addexternallogin",
    var changePasswordUrl = httpsUrl + "/api/account/changepassword",
        loginUrl = httpsUrl + "/token",
        //logoutUrl = httpsUrl + "/api/account/logout",
        registerUrl = httpsUrl + "/api/account/register",
        //registerExternalUrl = httpsUrl + "/api/account/registerrxternal",
        removeLoginUrl = httpsUrl + "/api/account/removelogin",
        setPasswordUrl = httpsUrl + "/api/account/setpassword",
        //siteUrl = httpsUrl + "/",
        userInfoUrl = httpsUrl + "/api/account/userinfo",
        sendResetPasswordMailUrl = httpsUrl + "/api/account/sendresetpasswordmail",
        resetPasswordUrl = httpsUrl + "/api/account/resetpassword",
        profileUrl = httpsUrl + "/api/account/profile";

    // Route operations
    function getExternalLoginsReturnUrl(returnData) {
        return encodeURIComponent("/?rd=" + encodeURIComponent(returnData));
    }

    function externalLoginsUrl(returnData, generateState) {
        return httpsUrl + "/api/account/externallogins?returnUrl=" + (getExternalLoginsReturnUrl(returnData)) +
            "&generateState=" + (generateState ? "true" : "false");
    }

    function manageInfoUrl(returnData, generateState) {
        return httpsUrl + "/api/account/manageinfo?returnUrl=" + (getExternalLoginsReturnUrl(returnData)) +
            "&generateState=" + (generateState ? "true" : "false");
    }

    // Other private operations
    //function getSecurityHeaders() {
    //    var accessToken = sessionStorage["accessToken"] || localStorage["accessToken"];

    //    if (accessToken) {
    //        return { "Authorization": "Bearer " + accessToken };
    //    }

    //    return {};
    //}

    return {
        
        //toErrorsArray: function (data) {
        //    var errors = new Array(),
        //        items;

        //    if (!data || !data.message) {
        //        return null;
        //    }

        //    if (data.modelState) {
        //        for (var key in data.modelState) {
        //            items = data.modelState[key];

        //            if (items.length) {
        //                for (var i = 0; i < items.length; i++) {
        //                    errors.push(items[i]);
        //                }
        //            }
        //        }
        //    }

        //    if (errors.length === 0) {
        //        errors.push(data.message);
        //    }

        //    return errors;
        //},

        // Data
        //returnUrl: siteUrl,

        // Data access operations
        //addExternalLogin: function (data) {
        //    return $.ajax(addExternalLoginUrl, {
        //        type: "POST",
        //        data: data,
        //        headers: getSecurityHeaders()
        //    });
        //},

        changePassword: function (data) {
            return $http({ method: "POST", url: changePasswordUrl, data: data });
        },

        getExternalLogins: function (returnUrl, generateState) {
            return $http({ method: "GET", url: externalLoginsUrl(returnUrl, generateState) })
                .then(function (response) {
                    return response.data;
                });
        },

        getManageInfo: function (returnUrl, generateState) {
            return $http({ method: "GET", url: manageInfoUrl(returnUrl, generateState) })
                .then(function (response) {
                    return response.data;
                });
        },

        getUserInfo: function () {
            return $http({ method: "GET", url: userInfoUrl });
        },

        login: function (data) {
            return $http({ method: "POST", url: loginUrl, data: data });
        },

        //logout: function () {
        //    return $.ajax(logoutUrl, {
        //        type: "POST",
        //        headers: getSecurityHeaders()
        //    });
        //},

        register: function (data) {
            return $http({ method: "POST", url: registerUrl, data: data });
        },

        //registerExternal: function (accessToken, data) {
        //    return $.ajax(registerExternalUrl, {
        //        type: "POST",
        //        data: data,
        //        headers: {
        //            "Authorization": "Bearer " + accessToken
        //        }
        //    });
        //},

        removeLogin: function (data) {
            return $http({ method: "POST", url: removeLoginUrl, data: data });
        },

        setPassword: function (data) {
            return $http({ method: "POST", url: setPasswordUrl, data: data });
        },

        sendResetPasswordMail: function (email) {
            return $http({ method: "GET", url: sendResetPasswordMailUrl + "?email=" + encodeURIComponent(email) });
        },

        resetPassword: function (email, newPassword, confirmPassword, token) {
            return $http({
                method: "POST",
                url: resetPasswordUrl,
                data: {
                    email: email,
                    newPassword: newPassword,
                    confirmPassword: confirmPassword,
                    token: token
                }
            });
        },

        getProfile: function () {
            return $http({ method: "GET", url: profileUrl });
        }
    };
}]);