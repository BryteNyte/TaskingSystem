using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using TaskingSystem.Models;

namespace TaskingSystem.Context
{
    public class CompanyContext
    {
        public List<CompanyModel> CompanyDropDown()
        {
            List<CompanyModel> cmp = new List<CompanyModel>();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TaskingConnection"].ToString()))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "Select cmp.cmpID,cmp.cmpName From CompanyTable cmp With (NOLOCK) where cmpActive = @Active ";
                    sqlCmd.Parameters.AddWithValue("@Active", 1);
                    //sqlCmd.Parameters.AddWithValue("@ID", id);
                    sqlCmd.Connection = sqlCon;
                    sqlCon.Open();

                    SqlDataReader sdr = sqlCmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        cmp.Add(new CompanyModel
                        {
                            cmpID = Convert.ToInt32(sdr["cmpID"]),
                            cmpName = sdr["cmpName"].ToString(),

                        });
                    }

                    return cmp;
                }
            }
            catch (Exception e)
            {
                return cmp;
            }



        }

        public List<CompanyModel> CompanyConnectionDropDown(int id)
        {
            List<CompanyModel> cmp = new List<CompanyModel>();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TaskingConnection"].ToString()))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "Select cp.cmpName,cp.cmpID ,ct.cndCompanyFrom,ct.cndCompanyTo From CompanyConnectionTable ct Left Join CompanyTable cp on ct.cndCompanyTo = cp.cmpID where ct.cndCompanyFrom =@ID and ct.cndStatus =1 and cp.cmpActive = @Active ";
                    sqlCmd.Parameters.AddWithValue("@Active", 1);
                    sqlCmd.Parameters.AddWithValue("@ID", id);
                    sqlCmd.Connection = sqlCon;
                    sqlCon.Open();

                    SqlDataReader sdr = sqlCmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        cmp.Add(new CompanyModel
                        {
                            cmpID = Convert.ToInt32(sdr["cmpID"]),
                            cmpName = sdr["cmpName"].ToString(),

                        });
                    }

                    return cmp;
                }
            }
            catch (Exception e)
            {
                return cmp;
            }



        }
    }
}