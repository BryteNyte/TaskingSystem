using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskingSystem.Models;

namespace TaskingSystem.ViewModels
{
    public class TasksViewModel
    {


        UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        public async Task<List<TasksModel>> GetTasks(int useID)
        {

            try
            {
                string color = "";
                using (HttpClient client = new HttpClient())
                {
                    List<TasksModel> taskModel = new List<TasksModel>();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetTaskList/ID="+ useID))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<List<TasksModel>>(textresponse);
                                foreach (var i in json)
                                {
                                    switch (i.tasStatus_Name)
                                    {
                                        case "Pending":
                                            color = "#57b5e3";
                                            break;
                                        case "In Progress":
                                            color = "#f4b400";
                                            break;
                                        case "On Hold":
                                            color = "#8c44a2";
                                            break;
                                        default:
                                            color = "#65b951";
                                            break;
                                    }
                                   
                                    taskModel.Add(new TasksModel
                                    {
                                        tasID = i.tasID,
                                        tasTitle = i.tasTitle,
                                        tasDelegatedBy_Name = i.tasDelegatedBy_Name,
                                        tasDelegatedTo_Name = i.tasDelegatedTo_Name,
                                        tasDelegatedBy_Company_Name = i.tasDelegatedBy_Company_Name,
                                        tasDue_Date_String = i.tasDue_Date_String,
                                        tasType_Name = i.tasType_Name,
                                        tasPriority_Name = i.tasPriority_Name,
                                        statusName = i.tasStatus_Name,
                                        tasDelegatedTo = i.tasDelegatedTo,
                                        tasDelagatedBy = i.tasDelagatedBy,
                                        tasStatus_Name = "<span style='background-color:"+ color + ";padding: 4px 6px 4px 6px;border-radius: 2px !important;background-clip:padding-box !important;color: #fff'>" + i.tasStatus_Name + "</span>" ,
                                        Action = "<a class='btn btn-info btn-xs' style='margin-top:6px'  href='" + helper.Action("TaskDetails", "Tasks", new { Id = i.tasID }) + "'>View</a>"

                                        //Action = "<a class='btn btn-info btn-xs' style='margin-top:6px'  href='" + helper.Action("UpdatePriority", "TaskPriority", new { Id = i.tasID }) + "'>View</a>" + "<a class='btn btn-danger btn-xs' style='margin-top:6px' onclick=' return confirm(\"Are you sure you wish to delete this record?\")' href='" + helper.Action("DeletePriority", "TaskPriority", new { Id = i.tasID }) + "'>Delete</a>"

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

        public async Task<bool> AddTask(TasksModel task)
        {
            //if (postedFile[0] != null)
            //{

            //    byte[] bytes;
            //    List<ImagesModel> img = new List<ImagesModel>();
            //    foreach (var item in postedFile)
            //    {
            //        using (BinaryReader br = new BinaryReader(item.InputStream))
            //        {
            //            bytes = br.ReadBytes(item.ContentLength);
            //        }
            //        img.Add(new ImagesModel
            //        {
            //            imgImage_Name = item.FileName,
            //            imgImage_Type = item.ContentType,
            //            imgImage_Image = bytes,
            //            imgType = 2,
            //        });
            //    }
            //    task.Images = img;



            //}
            try
            {

                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(task);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/AddTask/", httpContent);
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

        public async Task<TasksModel> TaskDetails(int ID)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    TasksModel tsk = new TasksModel();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/TaskDetails/ID=" + ID))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<TasksModel>(textresponse);

                                tsk.tasID = json.tasID;
                                tsk.tasTitle = json.tasTitle;
                                tsk.tasType_Name = json.tasType_Name;
                                tsk.tasStatus_Name = json.tasStatus_Name;
                                tsk.tasPriority_Name = json.tasPriority_Name;
                                tsk.tasDue_Date_String = json.tasDue_Date_String;
                                tsk.tasCreated_Date = json.tasCreated_Date;
                                tsk.tasDelegatedBy_Name = json.tasDelegatedBy_Name;
                                tsk.tasDelegatedTo_Name = json.tasDelegatedTo_Name;
                                tsk.Description = json.Description;
                                tsk.tasDelagatedBy = json.tasDelagatedBy;
                                tsk.tasDelegatedTo = json.tasDelegatedTo;
                                tsk.tasOrderNumber = json.tasOrderNumber;
                                tsk.tasDelegatedTo_Company = json.tasDelegatedTo_Company;
                            }
                            return tsk;
                        }
                        else
                        {
                            return tsk;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<ImagesModel>> GetImagesForTask(int id)
        {
            try
            {
            
                using (HttpClient client = new HttpClient())
                {
                    List<ImagesModel> img = new List<ImagesModel>();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] +"/api/GetImagesForTask/ID=" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<List<ImagesModel>>(textresponse);
                                foreach (var i in json)
                                {
                                    var imageType = i.imgImage_Name;
                                    imageType = imageType.Split('.').LastOrDefault();

                                    img.Add(new ImagesModel
                                    {
                                        imgImage_Name = i.imgImage_Name,
                                        imgImage_Type = imageType,
                                        imgID = i.imgID,
                                        imgPath_To_Image = ConfigurationManager.AppSettings["MySeverUrl"] + i.imgPath_To_Image,
                                        imgDownloadPath = ConfigurationManager.AppSettings["virtulUrl"] + Regex.Replace(i.imgPath_To_Image, "/", @"\"),
                                       
                                    });
                                }
                            }
                            return img;
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

        public async Task UpdateOrderNumber(int id,string number)
        {
            try
            {
                TasksModel tm = new TasksModel();
                tm.tasID = id;
                tm.tasOrderNumber = number;
                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(tm);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/UpdateOrderNumber/", httpContent);
                if (response.IsSuccessStatusCode)
                {

                    
                }
                else
                {
                    
                }

            }
            catch (Exception e)
            {
                
            }
        }

        public async Task UpdateDate(int id, DateTime date)
        {
            try
            {
                TasksModel tm = new TasksModel();
                tm.tasID = id;
                tm.tasDue_Date = date;
                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(tm);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/UpdateDate/", httpContent);
                if (response.IsSuccessStatusCode)
                {


                }
                else
                {

                }

            }
            catch (Exception e)
            {

            }
        }

        public async Task UpdateUser(int id, int user)
        {
            try
            {
                TasksModel tm = new TasksModel();
                tm.tasID = id;
                tm.tasDelegatedTo = user;
                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(tm);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/UpdateUser/", httpContent);
                if (response.IsSuccessStatusCode)
                {


                }
                else
                {

                }

            }
            catch (Exception e)
            {

            }
        }

        public async Task<int> GetTimeTaken(int ID)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    FeedbackModel tsk = new FeedbackModel();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetTimeTaken/ID=" + ID))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<FeedbackModel>(textresponse);

                                tsk.fdbTimeTaken = json.fdbTimeTaken;
                               
                            }
                            return Convert.ToInt32(tsk.fdbTimeTaken) ;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> GetRequiredTime(int ID)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    FeedbackModel tsk = new FeedbackModel();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetRequiredTime/ID=" + ID))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<FeedbackModel>(textresponse);

                                tsk.fdbRequiredTime = json.fdbRequiredTime;

                            }
                            return Convert.ToInt32(tsk.fdbRequiredTime);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<string> GetDate(int ID)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    TasksModel tsk = new TasksModel();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetDate/ID=" + ID))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<TasksModel>(textresponse);

                                tsk.tasDue_Date_String = json.tasDue_Date_String;

                            }
                            return tsk.tasDue_Date_String;
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

       

    }
}