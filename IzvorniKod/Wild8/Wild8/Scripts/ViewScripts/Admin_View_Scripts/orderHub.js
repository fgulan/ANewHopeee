$(document).ready(function() {
    setUpHub();    
});

function setUpHub() {
    var hub = $.connection.OrderHub;
    var con = $.connection.hub;

    hub.client.addNewOrder = function (order) { newOrderAdded(order) };

    hub.client.orderProcessed = function (orders) { populateOrderStorage(orders) };

    hub.client.populateOrderStorage = function (orders) { populateOrderStorage(orders); };

    con.start().done(function() {
        hub.server.joinWorkerGroup();   //Join worker group

        $('#orders-menu').on('click', function() {
            hub.server.getOrders(); //Populate orders with orders
        });
        
        $("#logout").one('click', function() {
            hub.server.leaveWorkerGroup();
        });
    });
}

//When order is added it is passed to all of active users 
//
function newOrderAdded(order) {
    //Increment order count badge
    var oldOrderCount = parseInt($('#orders-badge').text());
    $("#orders-badge").html(oldOrderCount + 1);

    //If user is in orders page call controler to return a view for single order 
    //And append it to orders panel
    var ordersMenu = $(document).find("#orders-menu");
    if (ordersMenu != undefined) {
        printOnOrderMenu(order, new Order($.parseJSON(order)));
    }
}

function printOnOrderMenu(order, dsOrder) {
    var hub = $.connection.OrderHub;
    var orderTemplate = $(generateOrderTemplate(dsOrder));
    orderTemplate.find(".accept-btn").on('click', function () {
        hub.server.orderProcessed(order);   //Let the server know that order has been processed
        
        var url = $(this).data('url');
        var employeeID = $(document).find("#employee-info").data("employee-id");
        var modal = $(document).find("#order-modal");
        modal.find("#modal-btn").on('click', function () {   //When user clicks the submit btn puhs order to controller
            var message = modal.find("#order-modal-message").val();
            $.ajax({
                type: 'POST',
                url: url,
                data: { orderJSON: order, employeeId: employeeID, message: message },
                success: function() {
                    orderTemplate.slideUp(600, function () { orderTemplate.remove() });
                    var oldOrderCount = parseInt($('#orders-badge').text());
                    if (oldOrderCount > 0) {
                        $("#orders-badge").html(oldOrderCount - 1);
                    }

                },   //Remove order from order list
                error: function () { window.alert('error while sending order to controller') }
            });
            modal.modal('hide');
        });
        modal.find("#order-modal-message").val(""); //Clear value in modal
        modal.modal('show');
    });
    orderTemplate.find(".refuse-btn").on('click', function () {
        hub.server.orderProcessed(order);   //Let the server know that order has been processed
        var modal = $(document).find("#order-modal");
        var url = $(this).data('url');
        modal.find("#modal-btn").on('click', function () {   //When user clicks the submit btn puhs order to controller
            var ta = modal.find("#order-modal-message");
            var msg = ta.text();
            var message = ta.val();
            $.ajax({
                type: 'POST',
                url: url,
                data: { message: message },
                success: function() {
                    orderTemplate.slideUp(600, function () { orderTemplate.remove() });
                    var oldOrderCount = parseInt($('#orders-badge').text());
                    if (oldOrderCount > 0) {
                        $("#orders-badge").html(oldOrderCount - 1);
                    }
                },   //Remove order from order list
                error: function () { window.alert('error while sending order to controller') }
            });
            modal.modal('hide');
        });
        modal.find("#order-modal-message").val(""); //Clear value in modal
        modal.modal('show');
    });
    $(document).find("#orders-list").append(orderTemplate).hide().slideDown(600);
}

//Prints all orders to the user
function populateOrderStorage(orders) {
    var ordersMenu = $("#orders-menu");
    $("#orders-list").empty();

    var jsonOrders = $.parseJSON(orders);
    var count = jsonOrders.length;

    $("#orders-badge").html(count); //Set badge coutn to propper value

    for (var i = 0; i < count; i++) {
        var dsOrder = new Order($.parseJSON(jsonOrders[i]));
        if (ordersMenu != undefined) {
            printOnOrderMenu(jsonOrders[i], dsOrder);
        }
    }
}

//Structures for checking if two orders are equal
function Order(order) {
    this.Name = order['Name'];
    this.Address = order['Address'];
    this.Phone = order['Phone'];
    this.Email = order['Email'];
    this.TotalPrice = order['TotalPrice'];
    this.UserNote = order["UserNote"];
    this.OrderTime = order['OrderTime'];
    this.Meals = [];

    for (var i = 0; i < order['Meals'].length; i++) {
        var meal = order['Meals'];
        this.Meals[i] = new Meal(meal[i]);
    }

    this.equals = function(other) {
        if(Name != other.Name) return false;
        if(Address != other.Address) return false;
        if(Phone != other.Phone) return false;
        if(Email != other.Email) return false;
        if(TotalPrice != other.TotalPrice) return false;
        if(UserNote != other.UserNote) return false;
        if(OrderTime != other.OrderTime) return false;
        
        if(Meals.length != other.Meals.length) return false;
        for(var i = 0; i < Meals.length; i++) {
            if(!Meals[i].equals(other.Meals[i])) return false;   
        }
        
        return true;
    } 
}

function Meal(meal) {
    this.MealName;
    this.MealTypeName; 
    this.Count;
    this.Addons;
    this.equals = function(other) {
        if(MealName != other.MealName) return false;
        if(MealTypeName != other.MealTypeName) return false;
        if(Count != other.Count) return false;
        if(Addons.length != other.Addons.length) return false;
        
        for(var i = 0; i < Addons.length; i++) {
            if(!Addons[i].equals(other.Addons[i])) return false;
        }
        
        return true;         
    }
     
    for(var prop in meal) {
        if(prop != 'Addons') {
            this[prop] = meal[prop];
        } else {
            this[prop] = new AddOns(meal[prop]);
        }        
    }
}

function AddOns(Addons) {
    this.Addons = Addons;
    this.equals = function(other) {
        if(Addons.length != other.addons.length) return false;
        
        for(var i = 0; i < Addons.length; i++) {
            if(Addons[i] != other.addons[i]) return false;
        }
        return true;
    };
}

function generateOrderTemplate(order) {
    var orderTamplate = $($("#order-tamplate").html()); //Get template
    orderTamplate.removeProp('hidden');
    orderTamplate.removeAttr('id');

    var orderHeading = "<h5>Adresa: " + order['Address'] + " | Tel: " + order['Phone'] + " | Cijena: " + order['TotalPrice'] + "</h5>";
    var header = orderTamplate.find("#order-heading");
    header.append(orderHeading);
    order['Meals'].forEach(function (meal) {
        var mealList = fillMealList(meal);
        orderTamplate.find("#order-details").append(mealList);
    });
    orderTamplate.find("#user-order-note").append("<p>" + order['UserNote'] + "</p>");
    return orderTamplate;
}

function fillMealList(meal) {
    var mealTamplate = $(document).find("#order-meal-template");
    mealTamplate.removeProp('hidden');
    mealTamplate.removeAttr('id');

    var mealHeading = "Jelo: " + meal['MealName'] + " | Veličina: " + meal["MealTypeName"];
    mealTamplate.find("#order-meal-name").html(mealHeading);
    var addons = meal['Addons']['Addons'];
    for(var i = 0; i < addons.length; i++){
        mealTamplate.find("#oreder-addon-list").append("<li>" + addons[i] + "</li>");
    }
    return mealTamplate;
}