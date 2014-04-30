function equalForKeys(a, b, keys) {
    if (!keys) {
        keys = [];
        for (var n in a) keys.push(n); // Used instead of Object.keys() for IE8 compatibility
    }

    for (var i = 0; i < keys.length; i++) {
        var k = keys[i];
        if (a[k] != b[k]) return false; // Not '===', values aren't necessarily normalized
    }

    return true;
}

function parseQueryString(queryString) {
    var data = {},
        pairs, pair, separatorIndex, escapedKey, escapedValue, key, value;

    if (queryString === null) {
        return data;
    }

    pairs = queryString.split("&");

    for (var i = 0; i < pairs.length; i++) {
        pair = pairs[i];
        separatorIndex = pair.indexOf("=");

        if (separatorIndex === -1) {
            escapedKey = pair;
            escapedValue = null;
        } else {
            escapedKey = pair.substr(0, separatorIndex);
            escapedValue = pair.substr(separatorIndex + 1);
        }

        key = decodeURIComponent(escapedKey);
        value = decodeURIComponent(escapedValue);

        data[key] = value;
    }

    return data;
}