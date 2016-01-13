﻿$(document).ready(function () {
    $("#orders-menu").on('click', function() {
        var url = $(this).data('url');
        $.ajax({
            type: 'GET',
            url: url,
            success: function (content) {
                $(".main-content").animate({
                    opacity: 0.0
                }, 600, function () {
                    $(this).empty();
                    $(this).html(content);
                    $(this).animate({
                        opacity: 1.0
                    }, 600, function() {
                        $.connection.OrderHub.server.getOrders();   //Populate orders
                    });
                });
            }
        });
    });
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
        printOnOrderMenu(order,$.parseJSON(order));
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
    orderTemplate.find(".open-btn").on('click', function() {
        var colapsable = orderTemplate.find(".collapse");
        var icon = orderTemplate.find(".open-icon");
        if (colapsable.hasClass("in")) { //Opened
            colapsable.collapse("toggle");
            icon.removeClass('glyphicon-menu-up').addClass('glyphicon-menu-down');
        } else if (!colapsable.hasClass("collapsing")) { //Closed
            colapsable.collapse("toggle");
            icon.removeClass('glyphicon-menu-down').addClass('glyphicon-menu-up');
        }
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
        var dsOrder = JSON.parse(jsonOrders[i]);
        if (ordersMenu != undefined) {
            printOnOrderMenu(jsonOrders[i], dsOrder);
        }
    }
}

function generateOrderTemplate(order) {
    var orderTamplate = $(getOrderTemplate()); //Get template

    var orderHeading = "<h5>Adresa: " + order['Address'] + " | Tel: " + order['PhoneNumber'] + " | Cijena: " + order['TotalPrice'] + "</h5>";
    var header = orderTamplate.find(".order-heading");
    header.append(orderHeading);
    var orderMealList = orderTamplate.find(".order-details");
    order['OrderDetails'].forEach(function (meal) {
        var mealList = fillMealList(meal);
        orderMealList.append(mealList);
    });
    orderTamplate.find(".user-order-note").append("<p>" + order['UserNote'] + "</p>");
    return orderTamplate;
}

function fillMealList(meal) {
    var mealTamplate = $(getMealTemplate());

    var mealHeading = "Jelo: " + meal['MealName'] + " | Veličina: " + meal["MealType"];
    mealTamplate.find(".order-meal-name").html(mealHeading);
    var addons = meal['OrderMealAddOns'];
    for(var i = 0; i < addons.length; i++){
        mealTamplate.find(".oreder-addon-list").append("<tr><td>" + i + "</td><td>" + addons[i]['AddOnName'] + "</td></tr>");
    }
    return mealTamplate;
}


function getOrderTemplate() {
    return '<div class="panel panel-default">' +
        '<div class="panel-heading">' +
        '<div class="panel-title pull-left">' +
        '<div class="order-heading"></div>' +
        '</div>' +
        '<div class="btn-group pull-right">' +
        '<button type="button" class="btn btn-sm btn-success accept-btn" data-url="/Admin/AcceptOrder">Potvrdi</button>' +
        '<button type="button" class="btn btn-sm btn-danger refuse-btn" data-url="/Admin/RefuseOrder">Odbij</button>' +
        '<button type="button" class="btn btn-sm btn-default open-btn">' +
        '<span class="glyphicon glyphicon-menu-down open-icon"></span>&nbsp;' +
        '</button>' +
        '</div>' +
        '<div class="clearfix"></div>' +
        '</div>' +
        '<div class="panel-content container-fluid">' +
        '<div class="collapse" data-toggle="collapse" aria-expanded="false">' +
        '<div class="panel-group order-details">' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="panel-footer user-order-note">' +
        '</div>' +
        '</div>';
}

function getMealTemplate() {
    return  '<div class="panel panel-default" >' +
            '    <div class="order-meal-name panel-heading" style="background: none"></div>' +
            '    <table class="table table-striped">' +
            '        <thead>' +
            '            <tr><th>#</th><th>Dodatci</th></tr>' +
            '        </thead>' +
            '        <tbody class="oreder-addon-list"></tbody>' +
            '    </table>' +
            '</div>';
}
