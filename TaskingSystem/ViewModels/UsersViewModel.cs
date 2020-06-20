using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskingSystem.ConnectionHelper;
using TaskingSystem.Models;

namespace TaskingSystem.ViewModels
{
    public class UsersViewModel
    {
        UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        public async Task<bool> Add(Users usr,HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                {
                    bytes = br.ReadBytes(postedFile.ContentLength);
                }
                ImagesModel images = new ImagesModel();
                images.imgImage_Image = bytes;
                images.imgImage_Type = postedFile.ContentType;
                images.imgType = 1;
                images.imgImage_Name = postedFile.FileName;

                usr.Images = images;

               
            }
            try
            {

                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(usr);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/AddUser/", httpContent);
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

        public async Task<List<Users>> Get()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    List<Users> usr = new List<Users>();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetUsers"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<List<Users>>(textresponse);
                                foreach (var i in json)
                                {
                                    usr.Add(new Users
                                    {
                                   useID = i.useID,
                                   useName = i.useName,
                                   useEmail = i.useEmail,
                                   useContact = i.useContact,
                                   useCompanyName = i.useCompanyName,
                                   useAccessLvlName = i.useAccessLvlName,
                                   Date = i.Date,
                                   Action = "<a class='btn btn-info btn-xs' style='margin-top:6px'  href='" + helper.Action("UpdatePriority", "TaskPriority", new { Id = i.useID }) + "'>Edit</a>" + "<a class='btn btn-danger btn-xs' style='margin-top:6px' onclick=' return confirm(\"Are you sure you wish to delete this record?\")' href='" + helper.Action("DeletePriority", "TaskPriority", new { Id = i.useID }) + "'>Delete</a>"

                                    });
                                }
                            }
                            return usr;
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


        public async Task<List<Users>> GetUserConnection(int cmpID)
        {

            try
            {

                using (HttpClient client = new HttpClient())
                {
                    List<Users> taskModel = new List<Users>();
                    using (HttpResponseMessage response = await client.GetAsync(Connection.GetApiConnectionString() + "/api/GetUserConnection/id=" + cmpID))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<List<Users>>(textresponse);

                               
                                foreach (var i in json)
                                {

                                    taskModel.Add(new Users
                                    {
                                        useID = i.useID,
                                        useName = i.useName,

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

    }
}