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
    public class AdminLevelsViewModel
    {
        UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        public async Task<bool> Add(AdminLevelModel adm)
        {
            try
            {

                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(adm);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/CreateAdminLevel/", httpContent);
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

        public async Task<List<AdminLevelModel>> Get()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    List<AdminLevelModel> adm = new List<AdminLevelModel>();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetAdminLevels"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<List<AdminLevelModel>>(textresponse);
                                foreach (var i in json)
                                {
                                    adm.Add(new AdminLevelModel
                                    {
                                        admID = i.admID,
                                        admName = i.admName,
                                        admDate_Created = i.admDate_Created,
                                        admLevel = i.admLevel,
                                        Action = "<a class='btn btn-info btn-xs' style='margin-top:6px'  href='" + helper.Action("Update", "AdminLevels", new { Id = i.admID }) + "'>Edit</a>" + "<a class='btn btn-danger btn-xs' style='margin-top:6px' onclick=' return confirm(\"Are you sure you wish to delete this record?\")' href='" + helper.Action("Delete", "AdminLevels", new { Id = i.admID }) + "'>Delete</a>"

                                    });
                                }
                            }
                            return adm;
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

        public async Task<AdminLevelModel> UpdateGet(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    AdminLevelModel adm = new AdminLevelModel();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/UpdateLevelGet/id=" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<AdminLevelModel>(textresponse);

                                adm.admID = json.admID;
                                adm.admName = json.admName;
                                adm.admLevel = json.admLevel;

                            }
                            return adm;
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

        public async Task<bool> Update(AdminLevelModel adm)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(adm);
                    using (HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        HttpResponseMessage response = null;
                        response = await httpClient.PutAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/UpdateAdminLevel/", httpContent);
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

        public async Task<bool> Delete(AdminLevelModel adm)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(adm);
                    using (HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        HttpResponseMessage response = null;
                        response = await httpClient.PutAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/DeleteAdminLevel/", httpContent);
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