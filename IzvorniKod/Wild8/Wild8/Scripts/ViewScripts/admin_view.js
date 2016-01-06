$(document).ready(function () {
    setOrdersMenuListener();
    setMealsMenuListener();
    setStatisticMenuListener();
    setWorkersMenuListener();
    setLogoutBtnListener();
});

function setOrdersMenuListener() {
    ajaxCall($("#orders-menu"), function (content) { replaceMainContent(content) }, notImplementedAlert);
};

function setMealsMenuListener() {
    ajaxCall($("#meal-menu"), function (content) { replaceMainContent(content); },notImplementedAlert);
};

function setStatisticMenuListener() {
    var caller = $("#statistic-menu");
    if (caller != undefined) {
        ajaxCall(caller, function (content) { replaceMainContent(content); }, notImplementedAlert);
    }
};

function setWorkersMenuListener() {
    var caller = $("#workers-menu");
    if (caller != undefined) {
        ajaxCall(caller, function (content) { replaceMainContent(content); } , notImplementedAlert);
    }
};

function setLogoutBtnListener() {
    $("#logout").click(function () {
        var actionUrl = $(this).data('url');
        
        $.ajax({
            type: 'POST',
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

function notImplementedAlert() {
    alert("Not implemented yet");
}

function ajaxCall(caller, success, error) {
    if (success == undefined) {
        success = function () { };
    }
    if (error == undefined) {
        error = function () { };
    }

    caller.click(function () {
        var url = $(this).data('url');

        $.ajax({
            type: 'POST',
            url: url,
            success: success(),
            error: error()
        })
    })
}