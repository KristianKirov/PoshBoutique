//poshBoutiqueApp.factory("detailsViewParentState", function ($modal, $state) {
//    return {
//        getStateData: function (match) {
//            if (match.length < 3) {
//                return null;
//            }

//            var stateUrl = match[1];
//            var itemUrl = match[2];
//            if (!stateUrl) stateUrl = "";
//            if (!itemUrl) itemUrl = "";

//            stateUrl = stateUrl.toLowerCase();
//            itemUrl = itemUrl.toLowerCase();

//            if (stateUrl.indexOf('/catalogue/') == 0) {
//                var categoryUrl = stateUrl.slice('/catalogue/'.length)

//                return {
//                    stateName: "catalogue.category",
//                    itemUrl: itemUrl,
//                    stateParams: {
//                        categoryUrl: categoryUrl
//                    }
//                }
//            }

//            return null;
//        }
//    }
//});

poshBoutiqueApp.factory("articleUrlProvider", function ($location) {
    var separator = "/view/";
    var separatorLength = separator.length;

    var getUrlSegmentsPrivate = function (url) {
        var currentUrl = decodeURIComponent(url || $location.url());
        
        var separatorIndex = currentUrl.indexOf(separator);
        if (separatorIndex == -1) {
            return { listUrl: currentUrl, itemUrl: null };
        }

        var listUrl = currentUrl.slice(0, separatorIndex);
        var itemUrl = currentUrl.slice(separatorIndex + separatorLength);

        return { listUrl: listUrl, itemUrl: itemUrl };
    };

    return {
        getUrlSegments: function (url) {
            return getUrlSegmentsPrivate(url);
        },
        getDetailsUrl: function (itemUrl, url) {
            var listUrl = getUrlSegmentsPrivate(url).listUrl;

            return listUrl + separator + itemUrl;
        },
        getListUrl: function (url) {
            return getUrlSegmentsPrivate(url).listUrl;
        },
        getItemUrl: function (url) {
            return getUrlSegmentsPrivate(url).itemUrl;
        }
    }
});