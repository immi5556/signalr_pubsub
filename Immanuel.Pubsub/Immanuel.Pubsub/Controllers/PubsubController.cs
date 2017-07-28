using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;

namespace Immanuel.Pubsub.Controllers
{
    public class PubsubController : ApiController
    {
        IHubContext<Hubs.IGroup> GetGroupHubContext()
        {
            return GlobalHost.ConnectionManager.GetHubContext<Hubs.GroupHub, Hubs.IGroup>();
        }

        IHubContext<Hubs.IUser> GetUserHubContext()
        {
            return GlobalHost.ConnectionManager.GetHubContext<Hubs.SingleHub, Hubs.IUser>();
        }

        [Route("api/{key}/group/{group}/send/{message}")]
        [HttpPost]
        public string SendToGroup(string key, string group, string message)
        {
            GetGroupHubContext().Clients.Group(group).SendToGroup(group, message);
            GetGroupHubContext().Clients.All.SendToGroup("", message);
            return $"Sent msg to Group - {group}";
        }

        [Route("api/{key}/group/{group}/add/{user}/{connectionid}")]
        [HttpPost]
        public string AddToGroup(string key, string group, string user, string connectionid)
        {
            //GetGroupHubContext().Groups.Add(connectionid, group);
            var gdata = Pubsub.Models.PubsubContext.GroupList.Find(t => t.GroupId == group);
            if (gdata == null)
            {
                gdata = new Models.UserGroup()
                {
                    GroupId = group
                };
                Pubsub.Models.PubsubContext.GroupList.Add(gdata);
            }
            var data = gdata.Users.Find(t => (t.ClientKey == key && t.UserName == user));
            if (data == null)
            {
                data = new Models.User();
                gdata.Users.Add(data);
            }
            data.UserName = user;
            data.IpAddr = GetIp();
            data.GroupId = group;
            data.ConnectionId = connectionid;
            data.ClientKey = key;
            data.Agent = HttpContext.Current.Request.UserAgent;
            Bll.PubQueue.PGrp.BeginInvoke(data, null, null);
            return $"Added {user} To Group({group}) Successful";
        }

        [Route("api/{key}/user/{user}/send/{message}")]
        [HttpPost]
        public string SendToUser(string key, string user, string message)
        {
            var mod = Pubsub.Models.PubsubContext.UsersList.Find(t => t.ClientKey == key && t.UserName == user);
            if (mod == null)
                return $"{user}, Not found..";
            GetUserHubContext().Clients.User(mod.ConnectionId).SendToUser(user, message);
            GetUserHubContext().Clients.All.SendToUser(user, message);
            return $"Sent msg to User - {user}";
        }

        [Route("api/{key}/user/{user}/add/{connectionid}")]
        [HttpGet]
        public string AddUser(string key, string user, string connectionid)
        {
            var data = Pubsub.Models.PubsubContext.UsersList.Find(t => (t.ClientKey == key && t.UserName == user));
            if (data == null)
            {
                data = new Models.User();
                Pubsub.Models.PubsubContext.UsersList.Add(data);
            }
            data.UserName = user;
            data.IpAddr = GetIp();
            data.ConnectionId = connectionid;
            data.ClientKey = key;
            data.Agent = HttpContext.Current.Request.UserAgent;
            Bll.PubQueue.PUser.BeginInvoke(data, null, null);
            return $"Added User - {user}";
        }

        [HttpGet]
        public string GetKey()
        {
            return RandomString(8);
        }

        [HttpGet]
        public dynamic GetKeys()
        {
            return new { appkey = RandomString(8), user = "usr-" + RandomString(3), group = "grp-" + RandomString(3) };
        }

        public string GetIp()
        {
            return GetClientIp();
        }

        private string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }

        protected string RandomString(int Size)
        {
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, Size)
                                   .Select(x => input[random.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }
        static Random random = new Random();
    }
}