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
    public class StatusContext
    {
        public List<StatusModel> StatusDropDown()
        {
            List<StatusModel> sta = new List<StatusModel>();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TaskingConnection"].ToString()))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "Select * From StatusTable With (NOLOCK)";
               
                    sqlCmd.Parameters.AddWithValue("@Active", 1);
                    sqlCmd.Connection = sqlCon;
                    sqlCon.Open();

                    SqlDataReader sdr = sqlCmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        sta.Add(new StatusModel
                        {
                            staID = Convert.ToInt32(sdr["staID"]),
                            staName = sdr["staName"].ToString(),

                        });
                    }

                    return sta;
                }
            }
            catch (Exception e)
            {
                return sta;
            }



        }
    }
}