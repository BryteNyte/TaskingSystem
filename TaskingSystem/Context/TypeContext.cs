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
    public class TypeContext
    {
        public List<TypeModel> TypeDropDown()
        {
            List<TypeModel> typ = new List<TypeModel>();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TaskingConnection"].ToString()))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "Select typName,typID From TaskType_Table With (NOLOCK) Where typActive = @Active ";
                    sqlCmd.Parameters.AddWithValue("@Active", 1);
                    sqlCmd.Connection = sqlCon;
                    sqlCon.Open();

                    SqlDataReader sdr = sqlCmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        typ.Add(new TypeModel
                        {
                            typID = Convert.ToInt32(sdr["typID"]),
                            typName = sdr["typName"].ToString(),

                        });
                    }

                    return typ;
                }
            }
            catch (Exception e)
            {
                return typ;
            }



        }
    }
}