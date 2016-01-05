$.connection.OrderHub.start().done(function () {
    //When connection is established add worker to WORKERS group
    //This needs to be executed when user logs in succesfully
    hub.server.joinWorkerGroup();
});