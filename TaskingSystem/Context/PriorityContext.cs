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
    public class PriorityContext
    {
        public List<PriorityModel> PriorityDropDown()
        {
            List<PriorityModel> pty = new List<PriorityModel>();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TaskingConnection"].ToString()))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "Select ptyName,ptyID From TaskPriority_Table With (NOLOCK) Where ptyActive = @Active ";
                    sqlCmd.Parameters.AddWithValue("@Active", 1);
                    sqlCmd.Connection = sqlCon;
                    sqlCon.Open();

                    SqlDataReader sdr = sqlCmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        pty.Add(new PriorityModel
                        {
                            ptyID = Convert.ToInt32(sdr["ptyID"]),
                            ptyName = sdr["ptyName"].ToString(),

                        });
                    }

                    return pty;
                }
            }
            catch (Exception e)
            {
                return pty;
            }



        }
    }
}