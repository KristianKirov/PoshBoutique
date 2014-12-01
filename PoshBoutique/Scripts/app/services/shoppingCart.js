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
    
    var getShippingPrice = function (priceWithoutShipping) {
        if (!cart.selectedDeliveryMethod) {
            return 0;
        }

        if (!cart.selectedPaymentMethod) {
            return cart.selectedDeliveryMethod.deliveryPrice;
        }

        if (!cart.selectedPaymentMethod.applyDeliveryTax) {
            return cart.selectedDeliveryMethod.deliveryPrice;
        }

        var shippingPrice = cart.selectedDeliveryMethod.deliveryPrice + ((cart.selectedDeliveryMethod.codTax * priceWithoutShipping) / 100);

        return shippingPrice;
    };

    var getSimpleOrderedItems = function () {
        var orderedItemsSimple = [];
        for (var i = 0; i < cart.items.length; i++) {
            var orderedItem = cart.items[i];
            var orderedItemSimple = {
                articleId: orderedItem.id,
                sizeId: orderedItem.size.id,
                quantity: orderedItem.quantity,
                price: orderedItem.price
            };

            if (orderedItem.color) {
                orderedItemSimple.colorId = orderedItem.color.id;
            }
            orderedItemsSimple.push(orderedItemSimple);
        }

        return orderedItemsSimple;
    };

    var cart = {
        items: orderdItems,
        addItemToCart: function (item, quantity, size, color) {
            var orderedItem = getOrderedItem(item, size, color);
            if (orderedItem) {
                orderedItem.quantity += quantity;
            }
            else {
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
        clean: function () {
            orderdItems.length = 0;
            shoppingCartPersistanceStorage.persistOrderedItems(orderdItems);
        },
        total: function () {
            var totalPrice = 0;
            for (var i = orderdItems.length - 1; i >= 0; i--) {
                var orderedItem = orderdItems[i];
                totalPrice += (orderedItem.quantity * orderedItem.price);
            }
            
            var shippingPrice = getShippingPrice(totalPrice);
            
            return {
                shipping: shippingPrice,
                order: totalPrice,
                full: (totalPrice + shippingPrice)
            };
        },
        isEmpty: function () {
            return !(orderdItems.length > 0);
        },
        steps: steps,
        getStepIndex: function (stepName) {
            for (var i = 0; i < steps.length; i++) {
                var step = steps[i];
                if (step.stateName === stepName) {
                    return i;
                }
            }

            return -1;
        },
        addressInfo: null,
        selectedPaymentMethod: null,
        selectedDeliveryMethod: null,
        getSimpleItems: function () {
            return getSimpleOrderedItems();
        },
        getSimpleOrder: function () {
            var simpleOrder = {};
            simpleOrder.items = getSimpleOrderedItems();
            simpleOrder.paymentMethodId = cart.selectedPaymentMethod.id;
            simpleOrder.deliveryMethodId = cart.selectedDeliveryMethod.id;
            simpleOrder.total = cart.total();

            return simpleOrder;
        }
    };

    orderStep.canShow = function () {
        return true;
    };

    addressStep.canShow = function () {
        return !cart.isEmpty();
    };

    delieryStep.canShow = function () {
        return !!cart.addressInfo;
    };

    paymentStep.canShow = function () {
        return !!cart.selectedDeliveryMethod;
    };

    confirmStep.canShow = function () {
        return !!cart.selectedPaymentMethod;
    };

    return cart;
});