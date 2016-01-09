$(document).ready(function () {
    $(".addToCartBtn").click(function () {
        $(this).attr("disabled", true); //Dible button
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
    });

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

    $(".trebamiime").click(function () {
        window.alert("Narudžba se obrađuje!");
    });
})