using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using DifferentiallyPrivate.Models;

namespace DifferentiallyPrivate.Services
{
    public class SmartStarInterface
    {
        string connectionString = ConfigurationManager.ConnectionStrings["RemoteSmartStarDB"].ConnectionString;

        public SmartStarInterface()
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM whatever");
        }

        public bool ValidateUser(User u)
        {
            using (var con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "GetPassword";
                cmd.Parameters.Add(new SqlParameter("username", u.UserName));
                cmd.Connection = con;

                con.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    var DBPassword = reader["password"];
                    if (DBPassword.ToString() == u.Password)
                    {
                        reader.Dispose();
                        cmd.Dispose();
                        return true;
                    }
                    else
                    {
                        reader.Dispose();
                        cmd.Dispose();
                        return false;
                    }
                }
                else
                {
                    reader.Dispose();
                    cmd.Dispose();
                    return false;
                }
            }
            return false;
        }
    }
}