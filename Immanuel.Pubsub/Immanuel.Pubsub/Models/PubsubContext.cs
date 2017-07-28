using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Immanuel.Pubsub.Models
{
    public static class PubsubContext
    {
        static PubsubContext()
        {
            UsersList = new List<User>();
            GroupList = new List<UserGroup>();
        }

        static void Init()
        {
            UsersList = Data.DataPubSub.GetUsers();
            GroupList = Data.DataPubSub.GetGroups();
        }

        public static List<User> UsersList { get; set; }
        public static List<UserGroup> GroupList { get; set; }
    }
}