using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Immanuel.Pubsub.Models
{
    public class UserGroup
    {
        public UserGroup()
        {
            Users = new List<User>();
        }

        public string GroupId { get; set; }
        public List<User> Users { get; set; }
    }
}