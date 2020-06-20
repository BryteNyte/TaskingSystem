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
    public class TaskTypeViewModel
    {
        UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        public async Task<bool> AddType(TypeModel typ)
        {
            try
            {

                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(typ);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/CreateTaskType/", httpContent);
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

        public async Task<List<TypeModel>> GetTaskType()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    List<TypeModel> typ = new List<TypeModel>();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetTaskType"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<List<TypeModel>>(textresponse);
                                foreach (var i in json)
                                {
                                    typ.Add(new TypeModel
                                    {
                                        typID = i.typID,
                                        typName = i.typName,
                                        typDate_Created = i.typDate_Created,
                                        Action = "<a class='btn btn-info btn-xs' style='margin-top:6px'  href='" + helper.Action("UpdateType", "TaskType", new { Id = i.typID }) + "'>Edit</a>" + "<a class='btn btn-danger btn-xs' style='margin-top:6px' onclick=' return confirm(\"Are you sure you wish to delete this record?\")' href='" + helper.Action("DeleteType", "TaskType", new { Id = i.typID }) + "'>Delete</a>"

                                    });
                                }
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

        public async Task<TypeModel> UpdateGet(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    TypeModel typ = new TypeModel();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/UpdateGet/id=" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<TypeModel>(textresponse);
                                
                                    typ.typID = json.typID;
                                    typ.typName = json.typName;
                              
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

        public async Task<bool> UpdateType(TypeModel typ)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(typ);
                    using (HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        HttpResponseMessage response = null;
                        response = await httpClient.PutAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/UpdateType/", httpContent);
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

        public async Task<bool> DeleteType(TypeModel typ)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(typ);
                    using (HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        HttpResponseMessage response = null;
                        response = await httpClient.PutAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/DeleteType/", httpContent);
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