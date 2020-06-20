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
    public class RoleContext
    {
        public List<AdminLevelModel> RoleDropDown()
        {
            List<AdminLevelModel> role = new List<AdminLevelModel>();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TaskingConnection"].ToString()))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "Select admLevel,admName From AdminLevel_Table With (NOLOCK) Where admActive = @Active ";
                    sqlCmd.Parameters.AddWithValue("@Active", 1);
                    sqlCmd.Connection = sqlCon;
                    sqlCon.Open();

                    SqlDataReader sdr = sqlCmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        role.Add(new AdminLevelModel
                        {
                            admLevel = Convert.ToInt32(sdr["admLevel"]),
                            admName = sdr["admName"].ToString(),

                        });
                    }

                    return role;
                }
            }
            catch (Exception e)
            {
                return role;
            }



        }
    }
}