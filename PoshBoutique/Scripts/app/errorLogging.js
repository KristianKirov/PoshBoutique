(function (window) {
    loggingEnabled = false;

    if (!loggingEnabled) {
        return;
    }

    window.scriptsToLoad = 2;

    function scriptLoaded() {
        window.scriptsToLoad = window.scriptsToLoad - 1;
        if (window.scriptsToLoad != 0) {
            return;
        }

        window.jslogger = new JSLogger();
        var loggedErrors = window.errorLogger || [];

        window.errorLogger = [];

        window.errorLogger.push = function (data) {
            Array.prototype.push.call(this, data);

            if (!data || !data.length || data.length <= 0) {
                return;
            }

            var loggerFunction = window.jslogger[data.shift()];

            if (!loggerFunction) {
                return;
            }

            loggerFunction.apply(window.jslogger, data);
        };

        for (var i = 0; i < loggedErrors.length; i++) {
            window.errorLogger.push(loggedErrors[i]);
        }
    }

    function loadScriptAsync(protocolAgnosticUrl) {
        var jsl = document.createElement('script'); jsl.type = 'text/javascript'; jsl.async = true;
        jsl.src = ('https:' == document.location.protocol ? 'https:' : 'http:') + protocolAgnosticUrl;
        jsl.onload = scriptLoaded;
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(jsl, s);
    }

    loadScriptAsync("//www.poshboutique-bg.com/scripts/stacktrace.min.js");
    loadScriptAsync("//jslogger.com/jslogger.js");
})(window);