poshBoutiqueApp.factory("shoppingCartPersistanceStorage", function ($window) {
    return {
        persistOrderedItems: function (orderedItems) {
            $window.localStorage["orderedItems"] = angular.toJson(orderedItems);
        },
        getOrderedItems: function () {
            var orderedItemsJson = $window.localStorage["orderedItems"];
            if (orderedItemsJson) {
                return angular.fromJson(orderedItemsJson);
            }

            return null;
        }
    }
});

poshBoutiqueApp.factory("shoppingCart", function (shoppingCartPersistanceStorage, $state) {
    var orderdItems = shoppingCartPersistanceStorage.getOrderedItems() || [];

    var getOrderedItemIndex = function (item, size, color) {
        for (var i = orderdItems.length - 1; i >= 0; i--) {
            var orderedItem = orderdItems[i];
            if (orderedItem.id == item.id && orderedItem.size.id == size.id) {
                if (!color || color.id == orderedItem.color.id) {
                    return i;
                }
            }
        }
    };

    var getOrderedItem = function (item, size, color) {
        var orderedItemIndex = getOrderedItemIndex(item, size, color);
        if (orderedItemIndex >= 0) {
            return orderdItems[orderedItemIndex];
        }

        return null;
    };

    var orderStep = {
        title: "Поръчка",
        stateName: "cart.order"
    };

    var addressStep = {
        title: "Адрес",
        stateName: "cart.address"
    };

    var delieryStep = {
        title: "Доставка",
        stateName: "cart.delivery"
    };

    var paymentStep = {
        title: "Плащане",
        stateName: "cart.payment"
    };

    var confirmStep = {
        title: "Потвърждаване",
        stateName: "cart.confirmation"
    };

    var steps = [orderStep, addressStep, delieryStep , paymentStep, confirmStep];

    return {
        items: orderdItems,
        addItemToCart: function (item, quantity, size, color) {
            var orderedItem = getOrderedItem(item, size, color);
            if (orderedItem) {
                orderedItem.quantity += quantity;
            }
            else {
                debugger;
                var cartItem = {
                    id: item.id,
                    title: item.title,
                    urlName: item.urlName,
                    thumbnailUrl: item.thumbnailUrl,
                    shortDescription: item.shortDescription,
                    price: item.price,
                    quantity: quantity,
                    size: size,
                    color: color
                };

                orderdItems.push(cartItem);
            }

            shoppingCartPersistanceStorage.persistOrderedItems(orderdItems);
        },
        removeItemFromCart: function (item) {
            var orderedItemIndex = getOrderedItemIndex(item, item.size, item.color);
            if (orderedItemIndex >= 0) {
                orderdItems.splice(orderedItemIndex, 1);
            }

            shoppingCartPersistanceStorage.persistOrderedItems(orderdItems);
        },
        persist: function () {
            shoppingCartPersistanceStorage.persistOrderedItems(orderdItems);
        },
        total: function () {
            var totalPrice = 0;
            for (var i = orderdItems.length - 1; i >= 0; i--) {
                var orderedItem = orderdItems[i];
                totalPrice += (orderedItem.quantity * orderedItem.price);
            }

            return totalPrice;
        },
        isEmpty: function () {
            return !(orderdItems.length > 0);
        },
        steps: steps
    };
});