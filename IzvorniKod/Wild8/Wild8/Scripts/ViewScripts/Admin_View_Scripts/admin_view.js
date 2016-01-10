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

$(document).on('click', '#addAddOnBtn', function (e) {
    e.preventDefault();
    $("#add-addon-form").ajaxForm({
        beforeSubmit: function (formData, jqForm, options) {
            return $("#add-addon-form").valid();
        },
        resetForm: false,
        cache: false,
        success: function (response) {
            printOnModal("Dodatak spremljen", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Dodatak nije spremljen", response);
        }
    });
    $("#add-addon-form").submit();
});

$(document).on('click', '#editAddOnBtn', function (e) {
    e.preventDefault();
    $("#edit-addon-form").ajaxForm({
        beforeSubmit: function (formData, jqForm, options) {
            return $("#edit-addon-form").valid();
        },
        resetForm: false,
        cache: false,
        success: function (response) {
            $("#OldName").val($("#Name").val());
            printOnModal("Dodatak spremljen", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Dodatak nije spremljen", response);
        }
    });
    $("#edit-addon-form").submit();
});

$(document).on('click', '#addMealBtn', function (e) {
    e.preventDefault();
    $("#add-meal-form").ajaxForm({
        beforeSubmit: function (formData, jqForm, options) {
            return $("#add-meal-form").valid();
        },
        resetForm: true,
        cache: false,
        success: function (response) {
            printOnModal("Jelo spremljeno", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Jelo nije spremljeno", response);
        }
    });
    $("#add-meal-form").submit();
});

$(document).on('click', '#editMealBtn', function (e) {
    e.preventDefault();
    $("#edit-meal-form").ajaxForm({
        beforeSubmit: function (formData, jqForm, options) {
            return $("#edit-meal-form").valid();
        },
        resetForm: false,
        cache: false,
        success: function (response) {
            printOnModal("Jelo spremljeno", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Jelo nije spremljeno", response);
        }
    });
    $("#edit-meal-form").submit();
});

$(document).on("click", ".meal-submenu", function () {
    $this = $(".meal-submenu");
    var url = $(this).data('url');

    $.ajax({
        type: 'GET',
        cache: false,
        url: url,
        success: function (content) {
            contentDiv = $("#meal-content");
            contentDiv.fadeOut(600, function () {
                contentDiv.empty();
                contentDiv.append(content).fadeIn(600);
            })
        },
        error: function () {
            window.alert('error');
        }
    });
});

$(document).on('click', '.del-addon-btn', function (e) {
    $("#delete-modal").remove();
    $("#del-addon-list").append('<div class="modal fade" id="delete-modal" role="dialog" aria-labelledby="basicModal" aria-hidden="true"></div>');

    $this = $(this);
    var url = $this.data('url');
    var addon_id = $this.data('addon-id');
    $("#delete-modal").load('/Admin/DeleteModal?Type=dodatak&Title=' + encodeURIComponent(addon_id));

    $("#delete-modal").on('click', '#modal-del-btn', function (e) {
        e.preventDefault();
        $.ajax({
            type: 'POST',
            cache: false,
            url: url,
            data: { AddOnID: addon_id },
            success: function () {
                $("#delete-modal").modal("hide");
                var parent = $this;
                parent.toggle("slow", function () {
                    parent.remove();
                });
            },
            error: function () {
                window.alert("Error");
            }
        });
    });
    $("#delete-modal").modal('show');
});

$(document).on('click', '.del-meal-btn', function (e) {
    e.preventDefault();
    $("#delete-modal").remove();
    $("#del-meal-list").append('<div class="modal fade" id="delete-modal" role="dialog" aria-labelledby="basicModal" aria-hidden="true"></div>');

    $this = $(this);
    var parent = $this;
    var url = $this.data('url');
    var meal_id = $this.data('meal-id');
    var meal_name = $this.data('meal-name');
    $("#delete-modal").load('/Admin/DeleteModal?Type=jelo&Title=' + encodeURIComponent(meal_name));

    $("#delete-modal").modal('show');

    $("#delete-modal").on('click', '#modal-del-btn', function (e) {
        e.preventDefault();
        $.ajax({
            type: 'POST',
            cache: false,
            url: url,
            data: { mealId: meal_id },
            success: function () {
                parent.toggle("slow", function () {
                    parent.remove();
                });
            },
            error: function () {
                window.alert("Error");
            }
        });
    });
});

$(document).on('click', '.edit-addon-btn', function () {
    var url = $(this).data('url');
    var addon_id = $(this).data('addon-id');

    $.ajax({
        type: 'GET',
        url: url,
        cache: false,
        data: { id: addon_id },
        success: function (content) {
            var listContainer = $('#edit-addon-list').parent();
            listContainer.fadeOut(600, function () {
                listContainer.empty();
                listContainer.append(content).fadeIn(600);
            });
        },
        error: function () {
            window.alert("Error");
        }
    })
});

$(document).on('click', '.edit-meal-btn', function () {
    var url = $(this).data('url');
    var meal_id = $(this).data('meal-id');

    $.ajax({
        type: 'GET',
        cache: false,
        url: url,
        data: { id: meal_id },
        success: function (content) {
            var listContainer = $('#edit-meal-list').parent();
            listContainer.fadeOut(600, function () {
                listContainer.empty();
                listContainer.append(content).fadeIn(600);
            });
        },
        error: function () {
            window.alert("Error");
        }
    })
});

$(document).on('click', '.deleteMealType', function (e) {
    if ($('.MealType').length == 1) return;
    $(this).closest('.MealType').fadeOut('fast', function () {
        $(this).remove();
    });;
});

function AddMealTypeInput() {
    var f = document.getElementsByClassName('MealType')[0].cloneNode(true);
    var elements = $(f).find('.form-control').each(function (index) {
        $(this).attr('value', '');
        $(this).val('');
        $(this).attr('defaultValue', '');
    });
    $(f).hide().appendTo($("#MealTypes")).fadeIn(600);
}

function printOnModal(title, content) {
    var modal = $("#modalInfoView");
    var headerLabel = modal.find("#modal-header-label");
    var bodyHeader = modal.find("#modal-body-label");

    headerLabel.html(title);
    bodyHeader.html(content);
    modal.modal('show');
}

$(document).on('click', '.open-btn', function (e) {
    var target = $($(this).data('target'));
    if (!target.hasClass('collapsing')) {
        var icon = $(this).children().first();
        if (icon.hasClass('glyphicon-menu-up')) {
            icon.removeClass('glyphicon-menu-up').addClass('glyphicon-menu-down');
        } else {
            icon.removeClass('glyphicon-menu-down').addClass('glyphicon-menu-up');
        }
    }
});

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