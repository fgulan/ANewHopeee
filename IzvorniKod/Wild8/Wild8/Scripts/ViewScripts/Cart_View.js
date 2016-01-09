$(document).ready(function() {
    disableOrderIfRestourantNotWorking();
    setRemoveBtnListeners();
    setCountChangeListener();
    setOrderBtnListener();
});

function disableOrderIfRestourantNotWorking() {
    //TODO:
    //Get current time,
    //Get restourant working hours 

    //If restourant is not working disable order button and set tooltip on it
    //so that user knows why he can not order meal


}

function setRemoveBtnListeners() {
    $(".removeFromCartBtn").one("click", function () { //Allowed only one click
        var parent = $(this).parent().parent(); //Get tr with all the info

        var mealId = parent.data('meal-id');
        var type = parent.data('type');
        var count = parent.data('count');
        var basePrice = parent.data('base-price');

        var addons = [];
        parent.find(".addon").each(function (index) {
            addons[index] = $(this).text();
        });

        var url = parent.data('url');

        $.ajax({
            type: 'POST',
            url: url,
            data: { MealID: mealId, Type: type, Count: count, AddOns: addons },
            cache: false,
            success: function (newCount) {
                parent.animate({
                    opacity: 0
                }, 500, function () {
                    $("#cartCount").html('<span class="glyphicon glyphicon-shopping-cart"></span> ' + newCount);
                    parent.remove();
                    var oldTotal = parseFloat($("#total-price").text().replace(",", "."));
                    var removed = parseFloat(basePrice.replace(",", "."));
                    $("#total-price").html((oldTotal - removed) + " HRK");
                });
            },
            error: function () {
                window.alert('error');
            }
        });
    }); //This is remove btn 

}

function setCountChangeListener() {
    $(".1-10").change(function () { //When size is changed
        var select = $(this);
        var parent = $(this).parent().parent(); //Get tr

        var mealId = parent.data('mealId');
        var type = parent.data('type');
        var oldCount = parent.data('count');
        var newCount = select.find(":selected").text();
        var basePrice = parent.data('base-price');

        var addons = [];
        parent.find(".addon").each(function (index) {
            addons[index] = $(this).text();
        });

        var url = parent.data('change-count-url');

        $.ajax({
            type: 'POST',
            url: url,
            data: { MealID: mealId, Type: type, oldCount: oldCount, AddOns: addons, newCount: newCount },
            cache: false,
            success: function (price) {
                var oldTotal = parseFloat($("#total-price").text().replace(",", "."));
                var newPrice = oldTotal - parseFloat(basePrice.replace(",", ".")) + parseFloat(price.replace(",", "."));
                $("#total-price").html(newPrice + " HRK");

                parent.data('count', newCount);
                parent.data('base-price', price);
                var orderPriceLabel = parent.find(".price");
                orderPriceLabel.html(price + " HRK");
            },
            error: function () {
            }
        });
    });
}

function setOrderBtnListener() {
    if (false) {
        $("#make-order-btn").click(function (e) {
            //Todo: validate input form 


            var hub = $.connection.OrderHub;
            hub.start().done(function () {
                var order = undefined;
                //Todo refractor this so that order accepts only neccesery order info

                var meals = [];
                $("#orders-list").find("tr").each(function (i) {
                    meals[i] = geMeal($(this));
                });
                if (orders.length == 0) {
                    //Show modal no orders have been placed
                    return;
                }


                //Todo get data from address form

                var clientInfo = {};

                var order = {
                    orders: orders,
                    clientInfo: clientInfo
                }

                var jsonOrder = JSON.stringify(order);

                hub.server.order(jsonOrder); //Push order to hub on server
                //Todo: replace order button and form with some picture or paragraph

                //Todo: remove all items from cart 
                hub.stop();
            });
        });
    }       //Just for now so that it does not fucks up working parts
}

//Order is 
//  mealId
//  mealTypeName
//  quantity
//  addon names *
function getMeal(parent) {
    var mealName = parent.data('meal-name');
    var mealType = parent.data('type');
    var count    = parent.data('count');

    var addons = [];
    parent.find(".addon").each(function(index) {
        addons[index] = $(this).text();
    });

    var order = {
        mealName:     mealName,
        mealTypeName: mealType,
        count:        count,
        addons:       addons
    }

    return order;
}