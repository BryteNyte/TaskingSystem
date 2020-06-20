using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TaskingSystem.ConnectionHelper;
using TaskingSystem.Models;

namespace TaskingSystem.ViewModels
{
    public class CompanyConnectionViewModel
    {
        public async Task<List<CompanyModel>> GetConnection(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    List<CompanyModel> cmp = new List<CompanyModel>();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetCompanyListConnection/id=" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<List<CompanyModel>>(textresponse);
                                foreach (var i in json)
                                {
                                    cmp.Add(new CompanyModel
                                    {
                                        cmpID = i.cmpID,
                                        cmpName = i.cmpName,
                                        shortAddress = i.streetNumber + " " + i.streetname + " " + i.suburb + " " + i.city,
                                        cndStatus = i.cndStatus,
                                        cndActionID = i.cndActionID
                                    });
                                }
                            }
                            return cmp;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<List<CompanyModel>> CompanyConnectionDropDown(int cmpID,string cmpName)
        {

            try
            {

                using (HttpClient client = new HttpClient())
                {
                    List<CompanyModel> taskModel = new List<CompanyModel>();
                    using (HttpResponseMessage response = await client.GetAsync(Connection.GetApiConnectionString() + "/api/GetCompanyConnection/id=" + cmpID))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<List<CompanyModel>>(textresponse);

                                taskModel.Insert(0, new CompanyModel()
                                {
                                    cmpID = cmpID,
                                    cmpName = cmpName
                                });
                                foreach (var i in json)
                                {

                                    taskModel.Add(new CompanyModel
                                    {
                                       cmpID = i.cmpID,
                                       cmpName = i.cmpName,
                                    });
                                }
                            }
                            return taskModel;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task AddConnection(int requested,int cmpID)
        {
            try
            {
                CompanyConnectionModel cnt = new CompanyConnectionModel();
                if (cmpID < requested)
                {
                    cnt.cndCmpId_one = cmpID;
                    cnt.cndCmpId_two = requested;
                }
                else
                {
                    cnt.cndCmpId_one = requested;
                    cnt.cndCmpId_two = cmpID;
                }
                cnt.cndActionID = cmpID;
                cnt.cndStatus = 2;
                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(cnt);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/AddConnection/", httpContent);
                

            }
            catch (Exception e)
            {
              
            }
        }

        public async Task AcceptConnection(int requested, int cmpID)
        {
            try
            {
                CompanyConnectionModel cnt = new CompanyConnectionModel();
                if (cmpID < requested)
                {
                    cnt.cndCmpId_one = cmpID;
                    cnt.cndCmpId_two = requested;
                }
                else
                {
                    cnt.cndCmpId_one = requested;
                    cnt.cndCmpId_two = cmpID;
                }
                cnt.cndActionID = cmpID;
                cnt.cndStatus = 1;
                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(cnt);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/AcceptConnection/", httpContent);


            }
            catch (Exception e)
            {

            }
        }
    }

}