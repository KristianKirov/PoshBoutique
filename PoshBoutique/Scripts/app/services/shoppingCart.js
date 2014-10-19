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
        stateName: "cart.order",
        isValid: function () {
            return orderdItems.length > 0;
        }
    };

    var addressStep = {
        title: "Адрес",
        stateName: "cart.address",
        prev: orderStep,
        isValid: function () {
            return false;
        }
    };

    var paymentStep = {
        title: "Плащане",
        stateName: "cart.payment",
        prev: addressStep,
        isValid: function () {
            return false;
        }
    };

    var confirmStep = {
        title: "Потвърждаване",
        stateName: "cart.confirmation",
        prev: paymentStep,
        isValid: function () {
            return false;
        }
    };

    var steps = [orderStep, addressStep, paymentStep, confirmStep];

    var maxIndexStep = 0;
    for (var i = 0; i < steps.length; i++) {
        var stateToValidate = steps[i];
        if (stateToValidate.isValid()) {
            ++maxIndexStep;
        }
        else {
            break;
        }
    }

    var getStepIndex = function (stateName) {
        for (var i = 0; i < steps.length; ++i) {
            var stepToCheck = steps[i];
            if (stepToCheck.stateName == stateName) {
                return i;
            }
        }

        return -1;
    }

    var canShowStep = function (stepIndex) {
        return stepIndex != -1 && stepIndex <= maxIndexStep;
    };

    var currentStepIndex = 0;

    return {
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
        steps: steps,
        validateCurrentDisplayStateOrRedirect: function () {
            var stepIndex = getStepIndex($state.$current.name);
            if (!canShowStep(stepIndex)) {
                $state.transitionTo(steps[0].stateName);
            }
            else {
                currentStepIndex = stepIndex;
            }
        },
        isStepActive: function (stepIndex) {
            return stepIndex <= currentStepIndex;
        },
        hasPrevStep: function () {
            return currentStepIndex > 0;
        },
        hasNextStep: function () {
            return currentStepIndex < steps.length - 1;
        },
        canGoToNextStep: function () {
            return canShowStep(currentStepIndex + 1);
        },
        goToPrevStep: function () {
            if (currentStepIndex > 0) {
                $state.transitionTo(steps[currentStepIndex - 1].stateName);
            }
        },
        goToNextStep: function () {
            if (canShowStep(currentStepIndex + 1)) {
                $state.transitionTo(steps[currentStepIndex + 1].stateName);
            }
        }
    };
});