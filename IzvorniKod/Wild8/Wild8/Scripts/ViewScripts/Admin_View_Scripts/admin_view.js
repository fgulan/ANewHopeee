$(document).ready(function () {
    if (typeof (Storage) == "undefined") {
        //Print to user that he can not use this browser 
    }

    ajaxCall($("#orders-menu"));
    ajaxCall($("#meal-menu"));    
    setStatisticMenuListener();
    setWorkersMenuListener();
    setLogoutBtnListener();
});

function setStatisticMenuListener() {
    var caller = $("#statistic-menu");
    if (caller != undefined) {
        ajaxCall(caller);
    }
};

function setWorkersMenuListener() {
    var caller = $("#workers-menu");
    if (caller != undefined) {
        ajaxCall(caller);
    }
};

function setLogoutBtnListener() {
    $("#logout").one('click', function () {
        var actionUrl = $(this).data('url');
        
        $.ajax({
            type: 'POST',
            cache: false,
            url: actionUrl,
            success: function () {
                var redirectUrl = $("#logout").data('index-url');
                window.location.href = redirectUrl;
            }
        })
    });
};

function replaceMainContent(content) {
    $(".main-content").animate({
        opacity: 0.0
    }, 600, function () {
        $(this).empty();
        $(this).html(content);
        $(this).animate({
            opacity: 1.0
        }, 600);
    });
}

function notImplementedAlert(xhr, status, errorMsg) {
    alert("Not implemented yet \n" + status + "\n" + errorMsg );
}

function ajaxCall(caller) {
    caller.click(function () {
        var url = $(this).data('url');

        $.ajax({
            type: 'GET',
            cache: false,
            url: url,
            success:  function (content) { replaceMainContent(content); },
            error: function (xhr, status, errorMsg) { notImplementedAlert(xhr,status, errorMsg)}
        })
    })
}

function setUpHub() {
    var hub = $.connection.OrderHub;

    hub.start().done(function() {

        hub.client.populateOrderStorage = function(orders, count) { saveOrders(orders); };

        hub.server.joinWorkerGroup();   //Join worker group

        hub.client.addNewOrder = function(order) { newOrderAdded(order) };

        hub.client.orderProcessed = function (order) { orderProcessed(order, hub) };

        $("#logout").one('click', function() {
            hub.server.leaveWorkerGroup();
        });
    });
}
//When order is added it is passed to all of active users 
//
function newOrderAdded(order) {
    //Save order to localStorage
    storeOrder(order);

    //Increment order count badge
    var oldOrderCount = parseInt($('#orders-badge').text());
    $("#orders-badge").html(oldOrderCount + 1);

    //If user is in orders page call controler to return a view for single order 
    //And append it to orders panel
    var ordersMenu = $(document).find("#orders-menu");
    if (ordersMenu != undefined) {
        var url = $('orders-menu').data('single-order-url');
        $.ajax({
            type: 'GET',
            url: url,
            data: order,
            success: function (orderPartial) {
                //Append order to order list 
                var ordersList = ordersMenu.find("#orders-list");
                $(orderPartial).css('display', 'none').appendTo(ordersList).slideDown('slow');
            },
            error: function(xhr, status, errorMsg) {
                
            }
        });
    }
}

//This function is used to allow multiple user to use system at the same time.
//When one user processes the order than all other users are notified and need 
//To remove that order from the list of orders.
//Process happens like this:
//  1.) user orders 
//  2.) notification of order comes to all of the users
//  3.) when one of the the user processes the order that notification goes to all other active users
//  4.) users removes the order from their session 
function orderProcessed(order, hub) {

}

//Prints all orders to the user 
function saveOrders(orders) {
    var jsOrdersObject = JSON.parse(orders);
    localStorage.setItem('orders', jsOrdersObject);
    $('#orders-badge').html(Object.keys(jsOrdersObject).length);
}

//Stores order in order array
function storeOrder(order) {
    var orders = localStorage.getItem('orders');
    var jsOrderObject = JSON.parse(order);
    orders.push(jsOrderObject);
    localStorage.setItem('orders', orders);
}