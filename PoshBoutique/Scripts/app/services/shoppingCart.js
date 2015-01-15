poshBoutiqueApp.factory("shoppingCartPersistanceStorage", ["$window", function ($window) {
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
}]);

poshBoutiqueApp.factory("shoppingCart", ["shoppingCartPersistanceStorage", "$state", function (shoppingCartPersistanceStorage, $state) {
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
                price: orderedItem.price,
                hasDiscount: orderedItem.hasDiscount
            };

            if (orderedItem.color) {
                orderedItemSimple.colorId = orderedItem.color.id;
            }
            orderedItemsSimple.push(orderedItemSimple);
        }

        return orderedItemsSimple;
    };

    var couponeExists = function (name) {
        for (var i = 0; i < cart.coupones.length; i++) {
            if (cart.coupones[i].name == name) {
                return true;
            }
        }

        return false;
    };

    var cart = {
        items: orderdItems,
        coupones: [],
        clearCoupones: function () {
            cart.coupones = [];
        },
        addCoupones: function (coupones) {
            for (var i = 0; i < coupones.length; i++) {
                var currentCoupone = coupones[i];
                if (!couponeExists(currentCoupone.name)) {
                    cart.coupones.push(currentCoupone);
                }
            }
            
            cart.refreshCouponeValues();
        },
        hasFreeShippingCoupon: function () {
            for (var i = 0; i < cart.coupones.length; i++) {
                if (cart.coupones[i].freeShipping) {
                    return true;
                }
            }

            return false;
        },
        refreshCouponeValues: function () {
            var cartTotal = cart.total();
            for (var i = 0; i < cart.coupones.length; i++) {
                var currentCoupone = cart.coupones[i];
                debugger;
                if (currentCoupone.valueType == 1) { //Percent
                    currentCoupone.absoluteValue = (cartTotal.orderNonDiscounted * currentCoupone.value) / 100;
                }
                else if (currentCoupone.valueType == 2) { //Absolute
                    currentCoupone.absoluteValue = currentCoupone.value;
                }
                else {
                    currentCoupone.absoluteValue = 0;
                }
            }
        },
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
                    color: color,
                    hasDiscount: item.hasDiscount
                };

                orderdItems.push(cartItem);
            }

            shoppingCartPersistanceStorage.persistOrderedItems(orderdItems);
            cart.refreshCouponeValues();
        },
        removeItemFromCart: function (item) {
            var orderedItemIndex = getOrderedItemIndex(item, item.size, item.color);
            if (orderedItemIndex >= 0) {
                orderdItems.splice(orderedItemIndex, 1);
            }

            shoppingCartPersistanceStorage.persistOrderedItems(orderdItems);
            cart.refreshCouponeValues();
        },
        persist: function () {
            shoppingCartPersistanceStorage.persistOrderedItems(orderdItems);
        },
        clean: function () {
            orderdItems.length = 0;
            cart.clearCoupones();
            shoppingCartPersistanceStorage.persistOrderedItems(orderdItems);
        },
        total: function () {
            var totalPrice = 0;
            var totalPriceNonDiscounted = 0;
            for (var i = orderdItems.length - 1; i >= 0; i--) {
                var orderedItem = orderdItems[i];
                var currentItemTotalPrice = orderedItem.quantity * orderedItem.price;
                totalPrice += currentItemTotalPrice;
                if (!orderedItem.hasDiscount) {
                    totalPriceNonDiscounted += currentItemTotalPrice;
                }
            }
            
            var shippingPrice = getShippingPrice(totalPrice);

            var couponesDiscount = 0;
            for (var i = 0; i < cart.coupones.length; i++) {
                couponesDiscount += cart.coupones[i].absoluteValue;
            }
            
            return {
                shipping: shippingPrice,
                order: totalPrice,
                orderWithDiscounts: totalPrice - couponesDiscount,
                orderNonDiscounted: totalPriceNonDiscounted,
                couponesDiscount: couponesDiscount,
                full: (totalPrice + shippingPrice - couponesDiscount)
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
            simpleOrder.coupones = cart.coupones;
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
}]);