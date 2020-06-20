using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskingSystem.Models;

namespace TaskingSystem.ViewModels
{
    public class AdminCompanyViewModel
    {
        UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        public async Task<bool> AddCompany(CompanyModel cmp)
        {
            try
            {

                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(cmp);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/CreateCompany/", httpContent);
                if (response.IsSuccessStatusCode)
                {

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                return false;
            }

        }

        public async Task<List<CompanyModel>> Get()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    List<CompanyModel> cmp = new List<CompanyModel>();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetAdminCompany"))
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
                                        cmpRegNumber = i.cmpRegNumber,
                                        cmpVatNumber = i.cmpVatNumber,
                                        cmpAddress = i.streetNumber + " " + i.streetname + " " + i.suburb + " " + i.city + " " + i.province + " " + i.code,
                                        cmpEmail = i.cmpEmail,
                                        Date = i.Date,
                                        cmpContactPersonName = i.cmpContactPersonName,
                                        cmpContactPersonNumber = i.cmpContactPersonNumber,
                                        shortAddress = i.streetNumber + " " + i.streetname + " " + i.suburb + " " + i.city,
                                        Action = "<a class='btn btn-info btn-xs' style='margin-top:6px'  href='" + helper.Action("UpdatePriority", "TaskPriority", new { Id = i.cmpID }) + "'>Edit</a>" + "<a class='btn btn-danger btn-xs' style='margin-top:6px' onclick=' return confirm(\"Are you sure you wish to delete this record?\")' href='" + helper.Action("DeletePriority", "TaskPriority", new { Id = i.cmpID }) + "'>Delete</a>"

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
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<PriorityModel> UpdateGet(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    PriorityModel typ = new PriorityModel();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/UpdateGetPriority/id=" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<PriorityModel>(textresponse);

                                typ.ptyID = json.ptyID;
                                typ.ptyName = json.ptyName;

                            }
                            return typ;
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

        public async Task<bool> UpdatePriority(PriorityModel typ)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(typ);
                    using (HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        HttpResponseMessage response = null;
                        response = await httpClient.PutAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/UpdatePriority/", httpContent);
                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> DeletePriority(PriorityModel typ)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(typ);
                    using (HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        HttpResponseMessage response = null;
                        response = await httpClient.PutAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/DeletePriority/", httpContent);
                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}