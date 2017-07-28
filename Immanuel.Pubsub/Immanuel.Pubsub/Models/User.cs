using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Immanuel.Pubsub.Models
{
    public class User
    {
        public long Id { get; set; }
        public string ClientKey { get; set; }
        public string UserName { get; set; }
        public string GroupId { get; set; }
        public string ConnectionId { get; set; }
        public string IpAddr { get; set; }
        public string Agent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}