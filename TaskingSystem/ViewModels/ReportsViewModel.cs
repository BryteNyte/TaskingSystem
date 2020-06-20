using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskingSystem.Models;

namespace TaskingSystem.ViewModels
{
    public class ReportsViewModel
    {
        UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        public async Task<List<TasksModel>> GetReports(int cmpID,int month)
        {

            try
            {
                string color = "";
                using (HttpClient client = new HttpClient())
                {
                    List<TasksModel> taskModel = new List<TasksModel>();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetReportsList/ID=" + cmpID + "/" + "date=" + month))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<List<TasksModel>>(textresponse);
                                foreach (var i in json)
                                {
                                    
                                    taskModel.Add(new TasksModel
                                    {
                                        tasDelegatedTo_Name = i.tasDelegatedTo_Name,
                                        tasDelegatedTo = i.tasDelegatedTo,
                                        tasTimeSpent = i.tasTimeSpent,
                                        tasWorked = i.tasWorked,
                                        tasDue_Date = i.tasDue_Date,
                                        Action = "<a class='btn btn-info btn-xs' style='margin-top:6px'  href='" + helper.Action("TaskDetails", "Tasks", new { Id = i.tasID }) + "'>View</a>"

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
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<List<TasksModel>> GetReportOverView(int cmpID,int day,int month,int year)
        {

            try
            {
                string color = "";
                using (HttpClient client = new HttpClient())
                {
                    List<TasksModel> taskModel = new List<TasksModel>();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetReportOverView/ID=" + cmpID + "/" + "day=" + day + "/" + "date=" + month + "/" + "year=" + year))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                if (textresponse.Count() > 0)
                                {
                                    var json = JsonConvert.DeserializeObject<List<TasksModel>>(textresponse);
                                    foreach (var item in json)
                                    {
                                        taskModel.Add(new TasksModel
                                        {
                                            tasDelegatedBy_Company_Name = item.tasDelegatedBy_Company_Name,
                                            tasWorked = item.tasWorked,
                                            tasDelegatedTo_Name = item.tasDelegatedTo_Name,
                                            tasDelegatedBy_Name = item.tasDelegatedBy_Name,
                                            tasDelegatedTo = item.tasDelegatedTo,
                                            tasTimeSpent = item.tasTimeSpent,
                                            tasDelagatedBy = item.tasDelagatedBy
                                       
                                    });
                                        
                                    }
                                    

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
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<List<TasksModel>> GetCompanyReports(int cmpID,int month,int year)
        {
            try
            {
              
                using (HttpClient client = new HttpClient())
                {
                    List<TasksModel> taskModel = new List<TasksModel>();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetCompanyReports/ID=" + cmpID + "/" + "date=" + month + "/" + "year="+ year))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<List<TasksModel>>(textresponse);
                                foreach (var i in json)
                                {

                                    taskModel.Add(new TasksModel
                                    {
                                        tasDelegatedBy_Company_Name = i.tasDelegatedBy_Company_Name,
                                        tasTimeSpent = i.tasTimeSpent,
                                        tasWorked = i.tasWorked,
                                        tasBillable = i.tasBillable,
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
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<List<TasksModel>> GetTaskReportDetails(int cmpID, int day, int month, int year)
        {

            try
            {
                string color = "";
                using (HttpClient client = new HttpClient())
                {
                    List<TasksModel> taskModel = new List<TasksModel>();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetTaskReportDetails/ID=" + cmpID + "/" + "day=" + day + "/" + "date=" + month + "/" + "year=" + year))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                if (textresponse.Count() > 0)
                                {
                                    var json = JsonConvert.DeserializeObject<List<TasksModel>>(textresponse);
                                    foreach (var item in json)
                                    {
                                        taskModel.Add(new TasksModel
                                        {
                                     
                                            tasTitle = "<a  style='color: #337ab7;text-decoration:underline; ' href='" + helper.Action("TaskDetails", "Tasks", new { Id = item.tasID }) + "'>" + item.tasTitle + "</a> <br/>" + item.tasDescription  ,
                                            tasDelegatedTo_Name = item.tasDelegatedTo_Name,
                                            tasDelegatedBy_Name = item.tasDelegatedBy_Name,
                                            tasDelegatedTo = item.tasDelegatedTo,
                                            tasTimeSpent = item.tasTimeSpent,
                                            tasDelagatedBy = item.tasDelagatedBy,
                                           tasTimeRequired = item.tasTimeRequired,
                                           tasOrderNumber = item.tasOrderNumber,
                                           tasDue_Date_String = item.tasDue_Date_String

                                        });

                                    }


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
            catch (Exception e)
            {
                return null;
            }

        }


    }
}