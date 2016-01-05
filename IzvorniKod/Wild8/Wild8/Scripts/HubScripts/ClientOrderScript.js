var hub = $.connection.OrderHub;

hub.client.orderAccepted = function (durnationTillArival) {
    //Todo render order bill to page
    //And arival time


    //Disconnect
    hub.stop();
};

hub.client.orderDeclined = function (message) {
    //Todo render decline message to page


    //Disconnect
    hub.stop();
};

hub.start().done(function () {
    $('.OrderButton').click(function () {
        var order = undefined;
        
        //Todo refractor this so that order accepts only neccesery order info
        //And then on server side create order object
        hub.server.order(order);    

        //Todo: add some king of animation while waiting for resoult
    });
});