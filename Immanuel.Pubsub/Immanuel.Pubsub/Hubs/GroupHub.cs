using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Immanuel.Pubsub.Hubs
{
    [HubName("GroupHub")]
    public class GroupHub : Hub<IGroup>
    {
        public void SendToGroup(string grp, string msg)
        {
            Clients.Group(grp).SendToGroup(grp, msg);
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

    public interface IGroup
    {
        void SendToGroup(string grp, string msg);
    }
}