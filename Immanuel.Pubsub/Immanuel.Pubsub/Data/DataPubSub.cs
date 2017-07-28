using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Immanuel.Pubsub.Data
{
    public class DataPubSub
    {
        public static List<Models.User> GetUsers()
        {
            List<Models.User> usr = new List<Models.User>();
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection("Server=198.38.83.33;Database=immanuel_kv;User Id=immanuel_sa;Password=12345;"))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
                {
                    cmd.CommandText = "select * from [PubSubUser]";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usr.Add(new Models.User()
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("Id")),
                                Agent = reader.GetString(reader.GetOrdinal("Agent")),
                                ClientKey = reader.GetString(reader.GetOrdinal("ClientKey")),
                                ConnectionId = reader.GetString(reader.GetOrdinal("ConnectionId")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                IpAddr = reader.GetString(reader.GetOrdinal("IpAddr")),
                                UserName = reader.GetString(reader.GetOrdinal("User"))
                            });
                        }
                    }
                }
            }
            return usr;
        }

        public static List<Models.UserGroup> GetGroups()
        {
            List<Models.UserGroup> grp = new List<Models.UserGroup>();
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection("Server=198.38.83.33;Database=immanuel_kv;User Id=immanuel_sa;Password=12345;"))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
                {
                    cmd.CommandText = "select * from [PubSubGroup]";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string grpid = reader.GetString(reader.GetOrdinal("GroupId"));
                            var grplst = grp.Find(t => t.GroupId == grpid);
                            if (grplst == null)
                            {
                                grp.Add(new Models.UserGroup()
                                {
                                    GroupId = grpid,
                                    Users = new List<Models.User>()
                                });
                            }
                            List<Models.User> usr = grp.Find(t => t.GroupId == grpid).Users;
                            usr.Add(new Models.User()
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("Id")),
                                Agent = reader.GetString(reader.GetOrdinal("Agent")),
                                ClientKey = reader.GetString(reader.GetOrdinal("ClientKey")),
                                ConnectionId = reader.GetString(reader.GetOrdinal("ConnectionId")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                IpAddr = reader.GetString(reader.GetOrdinal("IpAddr")),
                                UserName = reader.GetString(reader.GetOrdinal("User"))
                            });
                        }
                    }
                }
            }
            return grp;
        }
    }
}