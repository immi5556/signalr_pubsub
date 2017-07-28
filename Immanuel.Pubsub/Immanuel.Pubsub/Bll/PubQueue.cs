using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Immanuel.Pubsub.Bll
{
    public delegate void ProcUserQueue(Models.User user);
    public delegate void ProcGroupQueue(Models.User user);
    public class PubQueue
    {
        static PubQueue()
        {
            PUser += PrUqueue;
            PGrp += PrGqueue;
        }
        public static ProcUserQueue PUser { get; set; }
        public static ProcGroupQueue PGrp { get; set; }

        public static void PrUqueue(Models.User user)
        {
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection("Server=198.38.83.33;Database=immanuel_kv;User Id=immanuel_sa;Password=12345;"))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
                {
                    cmd.CommandText = "[immanuel_sa].[sp_UpdatePubsubUser]";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ip_ClientKey", user.ClientKey));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ip_UserName", user.UserName));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ip_ConnectionId", user.ConnectionId));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ip_IpAddr", user.IpAddr));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ip_Agent", user.Agent));
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void PrGqueue(Models.User user)
        {
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection("Server=198.38.83.33;Database=immanuel_kv;User Id=immanuel_sa;Password=12345;"))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
                {
                    cmd.CommandText = "[immanuel_sa].[sp_UpdatePubsubGroup]";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ip_ClientKey", user.ClientKey));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ip_GroupId", user.GroupId));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ip_UserName", user.UserName));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ip_ConnectionId", user.ConnectionId));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ip_IpAddr", user.IpAddr));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ip_Agent", user.Agent));
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}