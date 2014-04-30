poshBoutiqueApp.factory("detailsViewParentState", function ($modal, $state) {
    return {
        getStateData: function (match) {
            if (match.length < 3) {
                return null;
            }

            var stateUrl = match[1];
            var itemUrl = match[2];
            if (!stateUrl) stateUrl = "";
            if (!itemUrl) itemUrl = "";

            stateUrl = stateUrl.toLowerCase();
            itemUrl = itemUrl.toLowerCase();

            if (stateUrl.indexOf('/catalogue/') == 0) {
                var categoryUrl = stateUrl.slice('/catalogue/'.length)

                return {
                    stateName: "catalogue.category",
                    itemUrl: itemUrl,
                    stateParams: {
                        categoryUrl: categoryUrl
                    }
                }
            }

            return null;
        }
    }
});