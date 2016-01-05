var timeoutDeclineMessage = 'Trenutno nitko nije u mogučnosti preuzeti Vašu narudžbu';


var hub = $.connection.OrderHub;

hub.disconnected(function () {
    //Todo check if this is smart 
    //If you logout than this code should not execute so should be ok
    setTimeout(function () {
        $.connection.hub.start();
    }, 5000); // Restart connection after 5 seconds.
});

hub.client.addNewOrder = function (order, who) {
    //Todo store from who was the order and add order to the page
    //This method should be only called when connection is established
    //After all only server can call this method

    //Todo generate partial view for the order and add listeners to 
    //buttons

    $('.acceptOrder').click(function () {
        //TODO: Get aproximate time (do not accept if time is not entered correctly)
        var aproximateTime = undefined;

        hub.server.orderAccepted(who, aproximateTime);
    });

    $('.declineOrder').click(function () {
        var declineMessage = undefined;

        hub.server.declineOrder(who, declineMessage);
    });

    setTimeout(function () { //If after five minutes no replay 
        //Todo kill client connection

        hub.server.declineOrder(who, timeoutDeclineMessage);

        //Todo remove order from the screen
    }, 5 * 60 * 1000);
}
