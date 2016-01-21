$.connection.OrderHub.start().done(function () {
    //When connection is established and add event handler to logout class to remove 
    //Worker from WORKER group
    //This needs to be executed when user logs out
    $('.logout').click(function () {
        hub.server.leaveWorkerGroup();
        hub.stop();
    });
});