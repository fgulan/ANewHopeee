﻿using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Wild8.Hubs.Util;

namespace Wild8.Hubs
{
    [HubName("OrderHub")]
    public class OrderHub : Hub
    {
        public readonly static string WORKERS = "workers";

        /// <summary>
        /// This is the method that is called when user orders
        /// Order is sent to all workers currently connected
        /// </summary>
        /// <param name="order"> User order in json form </param>
        public void Order(string order)
        {
            //Call js method on all workers
            //Send message of order and name of the user that uses connection
            OrdersSet.GetInstance().Add(order);
            Clients.Group(WORKERS).addNewOrder(order);
        }

        /// <summary>
        /// This message is sent to client whose order is accepted.
        /// Worker js knows who sent order because that information was passed to 
        /// </summary>
        /// <param name="who"></param>
        public Task OrderProcessed(string order)
        {
            OrdersSet.GetInstance().Remove(order);
            return Clients.OthersInGroup(WORKERS).orderProcessed(order);
        }
        
        //This method should be called when worker has logged seccesfully 
        //It should send all of the active orders to the user
        public async Task JoinWorkerGroup()
        {
            await Groups.Add(Context.ConnectionId, WORKERS);
            var jsonOrders = JsonConvert.SerializeObject(OrdersSet.GetInstance().GetSet(), Formatting.Indented);
            Clients.Caller.populateOrderStorage(jsonOrders);
        }

        //This method should be called when worker logs out
        public Task LeaveWorkerGroup()
        {
            return Groups.Remove(Context.ConnectionId, WORKERS);
        }

    }
}