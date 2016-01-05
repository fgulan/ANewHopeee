using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Wild8.Models;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using Wild8.Hubs.Util;

namespace Wild8.Hubs
{
    [HubName("OrderHub")]
    public class OrderHub : Hub
    {
        public readonly static string WORKERS = "workers";
       
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        /// <summary>
        /// This is the method that is called when user orders
        /// Order is sent to all workers currently connected
        /// </summary>
        /// <param name="order"> User order </param>
        public void Order(Order order)
        {
            //Call js method on all workers
            //Send message of order and name of the user that uses connection
            Clients.Group(WORKERS).addNewOrder(order, Context.User.Identity.Name);
        }

        /// <summary>
        /// This message is sent to client whose order is accepted.
        /// Worker js knows who sent order because that information was passed to 
        /// </summary>
        /// <param name="who"></param>
        public void AcceptOrder(string who, string durnationTillArival)
        {
            var connectionId = _connections.GetConnection(who);
            if(connectionId == null)
            {
                return; //No connection for user 
            }

            Clients.Client(connectionId).orderAccepted(durnationTillArival);
        }

        /// <summary>
        /// Order can be decliend and message should be provided. 
        /// 
        /// Odred should be declined:
        ///      if restorant is closed, 
        ///      if worker does not answer to order in prescribed time
        ///      if worker cancles the order 
        /// All order refusal should have response message.
        /// 
        /// </summary>
        /// <param name="who"></param>
        /// <param name="message"></param>
        public void DeclineOrder(string who, string message)
        {
            var connectionId = _connections.GetConnection(who);
            if (connectionId == null)
            {
                return; //No connection for user 
            }

            Clients.Client(connectionId).orderDeclined(message);
        }
        
        //This method should be called when worker has logged seccesfully 
        public Task JoinWorkerGroup()
        {
            return Groups.Add(Context.ConnectionId, WORKERS);
        }

        //This method should be called when worker logs out
        public Task LeaveWorkerGroup()
        {
            return Groups.Remove(Context.ConnectionId, WORKERS);
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!_connections.GetConnection(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}