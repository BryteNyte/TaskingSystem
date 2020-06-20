using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TaskingSystem.Models;

namespace TaskingSystem.Context
{
    public class UserContext
    {
        public List<Users> UsersDropDown()
        {
            List<Users> usr = new List<Users>();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TaskingConnection"].ToString()))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "Select useID,useName,useSurname From UsersTable With (NOLOCK) Where  useActive = @Active ";
                    //sqlCmd.Parameters.AddWithValue("@ID", id);
                    sqlCmd.Parameters.AddWithValue("@Active", 1);
                    sqlCmd.Connection = sqlCon;
                    sqlCon.Open();

                    SqlDataReader sdr = sqlCmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        usr.Add(new Users
                        {
                            useID = Convert.ToInt32(sdr["useID"]),
                            useName = sdr["useName"].ToString() + " " + sdr["useSurname"].ToString(),

                        });
                    }

                    return usr;
                }
            }
            catch (Exception e)
            {
                return usr;
            }



        }

        public List<Users> UsersDropDownController(int id)
        {
            List<Users> usr = new List<Users>();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TaskingConnection"].ToString()))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "Select useID,useName,useSurname From UsersTable With (NOLOCK) Where useCompanyID = @ID AND  useActive = @Active ";
                    sqlCmd.Parameters.AddWithValue("@ID", id);
                    sqlCmd.Parameters.AddWithValue("@Active", 1);
                    sqlCmd.Connection = sqlCon;
                    sqlCon.Open();

                    SqlDataReader sdr = sqlCmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        usr.Add(new Users
                        {
                            useID = Convert.ToInt32(sdr["useID"]),
                            useName = sdr["useName"].ToString() + " " + sdr["useSurname"].ToString(),

                        });
                    }

                    return usr;
                }
            }
            catch (Exception e)
            {
                return usr;
            }



        }

        public List<Users> ToUsersDropDown(int id)
        {
            List<Users> use = new List<Users>();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TaskingConnection"].ToString()))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "Select u.useID,u.useName,u.useSurname From UsersTable u With (NOLOCK) LEFT JOIN CompanyConnectionTable cn ON cn.cndCompanyFrom = u.useCompanyID Where cn.cndCompanyTo = @ID and u.useActive = @Active ";
                    sqlCmd.Parameters.AddWithValue("@Active", 1);
                    sqlCmd.Parameters.AddWithValue("@ID", id);
                    sqlCmd.Connection = sqlCon;
                    sqlCon.Open();

                    SqlDataReader sdr = sqlCmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        use.Add(new Users
                        {
                            useID = Convert.ToInt32(sdr["useID"]),
                            useName = sdr["useName"].ToString() + " " + sdr["useSurname"].ToString(),

                        });
                    }

                    return use;
                }
            }
            catch (Exception e)
            {
                return use;
            }



        }

        public List<Users> UsersConnectionDropDown(int id)
        {
            List<Users> use = new List<Users>();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TaskingConnection"].ToString()))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "Select cp.cmpName ,ct.cndCompanyFrom,ct.cndCompanyTo,us.useName,us.useSurname,us.useID From CompanyConnectionTable ct Left Join CompanyTable cp on ct.cndCompanyTo = cp.cmpID Left Join UsersTable us on us.useCompanyID = cmpID where ct.cndCompanyFrom =@ID and ct.cndStatus =1 and us.useActive=@Active";
                    sqlCmd.Parameters.AddWithValue("@Active", 1);
                    sqlCmd.Parameters.AddWithValue("@ID", id);
                    sqlCmd.Connection = sqlCon;
                    sqlCon.Open();

                    SqlDataReader sdr = sqlCmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        use.Add(new Users
                        {
                            useID = Convert.ToInt32(sdr["useID"]),
                            useName = sdr["useName"].ToString() + " " + sdr["useSurname"].ToString(),

                        });
                    }

                    return use;
                }
            }
            catch (Exception e)
            {
                return use;
            }
        }
    }
}