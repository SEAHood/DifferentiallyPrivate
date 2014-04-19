using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using DifferentiallyPrivate.Models;

namespace DifferentiallyPrivate.Services
{
    public class DBInterface
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RemoteSmartStarDB"].ConnectionString;
        //private string connectionString = ConfigurationManager.ConnectionStrings["LocalSmartStarDB"].ConnectionString;

        public DBInterface()
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM whatever");
        }

        public bool ValidateUser(User u)
        {
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetPassword";
                    cmd.Parameters.Add(new SqlParameter("username", u.UserName));
                    cmd.Connection = con;

                    //ADD ERROR HANDLING
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
            }
            catch (Exception)
            {
                return false;
            }
        }

        public double[] GetDoublesFromDB(string category, string timespan)
        {
            using (var con = new SqlConnection(connectionString))
            {
                List<double> data = new List<double>();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetDoubleData";
                    cmd.Parameters.Add(new SqlParameter("category", category));
                    cmd.Parameters.Add(new SqlParameter("timespan", Int32.Parse(timespan)));
                    cmd.Connection = con;

                    //ADD ERROR HANDLING
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        double val = (double)reader.GetDecimal(0);
                        //double singleValue = reader.GetDouble(0);//(double)reader["output"];
                        data.Add(val);
                    }

                    reader.Dispose();
                    con.Close();
                    cmd.Dispose();
                    return data.ToArray();
                }
                catch (Exception ex)
                {
                    con.Close();
                    return null;
                }
            }
        }
    }
}