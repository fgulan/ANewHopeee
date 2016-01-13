$(document).ready(function() {
    setRemoveBtnListeners();
    setCountChangeListener();
    setOrderBtnListener();
});

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
        $("#make-order-btn").click(function (e) {
            e.preventDefault();

            var form = $("#order-form");
            form.validate({
                rules: {
                    client_email: {
                        required: true,
                        email: true
                    }
                }
            })
            if (!form.valid()) {
                return;
            }

            var startTime = form.data('start-time');
            var endTime   = form.data('end-time');
            //check if restourant is working
           if (!checkWorkingHours(startTime, endTime)) {
                printOnModal('Ups','Restoran trenutno ne radi.<br>Radno verijeme restorana je od ' + startTime + ' do ' + endTime);
                return;
           }
            

            //Make order (if order undifined that means it has no meals in it)
            var order = makeOrder();
            if (order == undefined) {
                printOnModal('Narudžba zabranjena','Nemate niti jedno jelo u košarici.')
                return;
            }

            var url = $(this).data('url');
            $.ajax({
                type: 'POST',
                url: url,
                data: { JsonOrder: JSON.stringify(order) },
                success: function(thankYouMsg) {
                    $("#dvCategoryResults").slideUp('slow', function() {
                        $(this).empty().html(thankYouMsg).hide().slideDown('slow');
                    });

                    //Set cart count to 0
                    $("#cartCount").html('<span class="glyphicon glyphicon-shopping-cart"></span> 0');
                }
            });
        });
}

function makeOrder() {
    var meals = [];

    $("#orders-list").find("tr").each(function (i) {
        meals[i] = getMeal($(this));
    });

    if (meals.length === 0) {
        //Show modal no orders have been placed
        return undefined;
    }


    //Get data from address form
    var name = $("#client-name").val();
    var addr = $("#client-address").val();
    var phone = $("#client-phone").val();
    var email = $("#client_email").val();
    var note = $("#client-note").val();
    var total = parseFloat($("#total-price").text().replace(",", "."));

    if (note == undefined) {
        note = '';
    }

    var order = {
        Name: name,
        Address: addr,
        PhoneNumber: phone,
        Email: email,
        TotalPrice: total,
        UserNote: note,
        OrderDetails: meals
    };

    return order;
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
        addons[index] = { AddOnName: $(this).text() }
    });

    var order = {
        MealName: mealName,
        MealType: mealType,
        Count: count,
        OrderMealAddOns: addons
    }

    return order;
}

function checkWorkingHours(startTime, endTime) {
    var f = checkTime(startTime, true);
    var s = checkTime(endTime, false);
    return f && s;
}

function checkTime(time, start) {
    var t = time.trim().split(":");

    var now = new Date();
    var h = now.getHours();
    var m = now.getMinutes();

    var hours = parseInt(t[0]);
    var min   = parseInt(t[1]);
    if (start) {
        if (hours < h) return true;
        if (hours === h && min <= m) return true;
        return false;
    } else {
        if (hours > h) return true;
        if (hours === h && min >= m) return true;
        return false;
    }
}

function printOnModal(title, content) {
    var modal = $("#cart-modal");
    modal.find(".modal-title").html(title);
    modal.find(".modal-body").html('<p>' + content + "</p>");
    modal.modal('show');
}