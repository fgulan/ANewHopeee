var hub = $.connection.OrderHub;

hub.start().done(function () {
    $('.OrderButton').click(function () {
        var order = undefined;
        
        //Todo refractor this so that order accepts only neccesery order info
        //And then on server side create order object
        //Serilize to json and send 


        hub.server.order(order);    
        //Todo: add some king of animation while waiting for resoult
    });
});