using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace Immanuel.Pubsub.Hubs
{
    [HubName("SingleHub")]
    public class SingleHub : Hub<IUser>
    {
        public void SendToUser(string user, string msg)
        {
            Clients.User(user).SendToUser(user, msg);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }

    public interface IUser
    {
        void SendToUser(string user, string msg);
    }
}