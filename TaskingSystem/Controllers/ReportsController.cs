using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskingSystem.Models;
using System.Linq.Dynamic;
using TaskingSystem.ViewModels;
using TaskingSystem.Context;

namespace TaskingSystem.Controllers
{
    public class ReportsController : Controller
    {
        ReportsViewModel rpVM = new ReportsViewModel();
        UsersViewModel usVm = new UsersViewModel();
        // GET: Reports
        public async Task<ActionResult> TasksReports()
        {
            var us = Session["useDetails"] as Users;
            List<Users> usrModel = new List<Users>();
            UserContext useContext = new UserContext();
            usrModel = await usVm.GetUserConnection(us.useCompanyID);
            TasksModel model = new TasksModel();
            List<DateListModel> dt = new List<DateListModel>();
            dt.Add(new DateListModel {value=1,DateName="January" });
            dt.Add(new DateListModel {value=2,DateName="February" });
            dt.Add(new DateListModel {value=3,DateName="March" });
            dt.Add(new DateListModel {value=4,DateName="April" });
            dt.Add(new DateListModel {value=5,DateName="May" });
            dt.Add(new DateListModel {value=6,DateName="June" });
            dt.Add(new DateListModel {value=7,DateName="July" });
            dt.Add(new DateListModel {value=8,DateName="August" });
            dt.Add(new DateListModel {value=9,DateName="Septmeber" });
            dt.Add(new DateListModel {value=10,DateName="October" });
            dt.Add(new DateListModel {value=11,DateName="November" });
            dt.Add(new DateListModel {value=12,DateName="December" });
            foreach (var item in dt)
            {
                model.DateList.Add(new SelectListItem { Text = item.DateName, Value = item.value.ToString() });
            }
            foreach (var user in usrModel)
            {
                model.Users.Add(new SelectListItem { Text = user.useName, Value = user.useID.ToString() });
            }
            model.tasID = DateTime.Now.Month;
            return View(model);
        }

        public async Task<ActionResult> GetReports()
        {
            try
            {
                var useID = 0;

                var item = Session["useDetails"] as Users;
                useID = item.useID;

                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortCulmn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0;
                var date = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
                var to = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
                int month = 0;
                if (string.IsNullOrWhiteSpace(date))
                {
                    month = 0;
                }
                else
                {
                    month = Convert.ToInt32(date);
                }
                 

                List<TasksModel> taskModel = new List<TasksModel>();
                taskModel = await rpVM.GetReports(item.useCompanyID, month);

                if (!string.IsNullOrWhiteSpace(to))
                {
                    taskModel = taskModel.Where(a => a.tasDelegatedTo == Convert.ToInt32(to)).ToList();
                }

                if (!(string.IsNullOrEmpty(sortCulmn + "" + sortColumnDir)))
                {
                    taskModel = taskModel.OrderBy(sortCulmn + " " + sortColumnDir).ToList();
                }
                totalRecords = taskModel.Count();
                var data = taskModel.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return View();
            }
           
        }

        public async Task<ActionResult> GetReportOverView()
        {
            try
            {
                var useID = 0;

                var item = Session["useDetails"] as Users;
                useID = item.useID;

                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortCulmn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0;
                var date = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
                var to = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
                var by = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
                int monthint = 0;
                string dtName = "";

                if (string.IsNullOrWhiteSpace(date))
                {
                    monthint = DateTime.Now.Month;
                }
                else
                {
                    monthint = Convert.ToInt32(date);
                }
                if (string.IsNullOrWhiteSpace(date))
                {
                    dtName = DateTime.Now.ToString("MMMM");
                }
                else
                {
                    switch (date)
                    {
                        case "1":
                            dtName = "Jan";
                            break;
                        case "2":
                            dtName = "Feb";
                            break;
                        case "3":
                            dtName = "Mar";
                            break;
                        case "4":
                            dtName = "Apr";
                            break;
                        case "5":
                            dtName = "May";
                            break;
                        case "6":
                            dtName = "June";
                            break;
                        case "7":
                            dtName = "July";
                            break;
                        case "8":
                            dtName = "Aug";
                            break;
                        case "9":
                            dtName = "Sept";
                            break;
                        case "10":
                            dtName = "Oct";
                            break;
                        case "11":
                            dtName = "Nov";
                            break;
                        case "12":
                            dtName = "Dec";
                            break;
                        default:
                            break;
                    }
                }
              

                List<TasksModel> taskModel = new List<TasksModel>();
                List<TasksModel> ts = new List<TasksModel>();
                for (int i = 1; i < DateTime.DaysInMonth(DateTime.Now.Year, monthint) + 1; i++)
                {

                    ts = await rpVM.GetReportOverView(item.useCompanyID, i, monthint, DateTime.Now.Year);
                    if (ts.Count > 0)
                    {
                        foreach (var it in ts)
                        {
                            taskModel.Add(new TasksModel
                            {
                                tasDelegatedBy_Company_Name = it.tasDelegatedBy_Company_Name,
                                tasCreated_Date = i + " " + dtName + " " + DateTime.Now.Year,
                                tasDelegatedBy_Name = it.tasDelegatedBy_Name,
                                tasDelegatedTo_Name = it.tasDelegatedTo_Name,
                                tasTimeSpent = it.tasTimeSpent,
                                tasWorked = it.tasWorked,
                                tasID = i,
                                tasDelegatedTo = it.tasDelegatedTo,
                                tasDelagatedBy = it.tasDelagatedBy,
                                
                            });
                        }
                       
                    }
                    
                }
               
                if (!string.IsNullOrWhiteSpace(to))
                {
                    taskModel = taskModel.Where(a => a.tasDelegatedTo == Convert.ToInt32(to)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(by))
                {
                    taskModel = taskModel.Where(a => a.tasDelagatedBy == Convert.ToInt32(by)).ToList();
                }

                if (!(string.IsNullOrEmpty(sortCulmn + "" + sortColumnDir)))
                {
                    taskModel = taskModel.OrderBy(sortCulmn + " " + sortColumnDir).ToList();
                }
                totalRecords = taskModel.Count();
                var data = taskModel.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return View();
            }

        }

        public async Task<ActionResult> GetCompanyTaskTime()
        {
            try
            {
                var useID = 0;

                var item = Session["useDetails"] as Users;
                useID = item.useID;

                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortCulmn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0;
                var date = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
                int month = 0;
                if (string.IsNullOrWhiteSpace(date))
                {
                    month = 0;
                }
                else
                {
                    month = Convert.ToInt32(date);
                }


                List<TasksModel> taskModel = new List<TasksModel>();
                taskModel = await rpVM.GetCompanyReports(item.useCompanyID, month,DateTime.Now.Year);



                if (!(string.IsNullOrEmpty(sortCulmn + "" + sortColumnDir)))
                {
                    taskModel = taskModel.OrderBy(sortCulmn + " " + sortColumnDir).ToList();
                }
                totalRecords = taskModel.Count();
                var data = taskModel.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return View();
            }

        }

        public async Task<ActionResult> TaskReportDetails()
        {
            var us = Session["useDetails"] as Users;
            List<Users> usrModel = new List<Users>();
            UserContext useContext = new UserContext();
            usrModel = await usVm.GetUserConnection(us.useCompanyID);
            TasksModel model = new TasksModel();
            List<DateListModel> dt = new List<DateListModel>();
            dt.Add(new DateListModel { value = 1, DateName = "January" });
            dt.Add(new DateListModel { value = 2, DateName = "February" });
            dt.Add(new DateListModel { value = 3, DateName = "March" });
            dt.Add(new DateListModel { value = 4, DateName = "April" });
            dt.Add(new DateListModel { value = 5, DateName = "May" });
            dt.Add(new DateListModel { value = 6, DateName = "June" });
            dt.Add(new DateListModel { value = 7, DateName = "July" });
            dt.Add(new DateListModel { value = 8, DateName = "August" });
            dt.Add(new DateListModel { value = 9, DateName = "Septmeber" });
            dt.Add(new DateListModel { value = 10, DateName = "October" });
            dt.Add(new DateListModel { value = 11, DateName = "November" });
            dt.Add(new DateListModel { value = 12, DateName = "December" });
            foreach (var item in dt)
            {
                model.DateList.Add(new SelectListItem { Text = item.DateName, Value = item.value.ToString() });
            }
            foreach (var user in usrModel)
            {
                model.Users.Add(new SelectListItem { Text = user.useName, Value = user.useID.ToString() });
            }
            model.tasID = DateTime.Now.Month;
            return View(model);
        }

        public async Task<ActionResult> GetTaskReportDetails()
        {
            try
            {
                var useID = 0;

                var item = Session["useDetails"] as Users;
                useID = item.useID;

                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortCulmn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0;
                var date = Request.Form.GetValues("columns[7][search][value]").FirstOrDefault();
                
                int monthint = 0;
         

                if (string.IsNullOrWhiteSpace(date))
                {
                    monthint = DateTime.Now.Month;
                }
                else
                {
                    monthint = Convert.ToInt32(date);
                }
                

                List<TasksModel> taskModel = new List<TasksModel>();
                List<TasksModel> ts = new List<TasksModel>();

                    ts = await rpVM.GetTaskReportDetails(item.useCompanyID, 0, monthint, DateTime.Now.Year);
                    if (ts.Count > 0)
                    {
                        foreach (var it in ts)
                        {
                            taskModel.Add(new TasksModel
                            {

                                tasDelegatedBy_Name = it.tasDelegatedBy_Name,
                                tasDelegatedTo_Name = it.tasDelegatedTo_Name,
                                tasTimeSpent = it.tasTimeSpent,
                                tasTimeRequired = it.tasTimeRequired,
                                tasOrderNumber = it.tasOrderNumber,
                                tasDue_Date_String = it.tasDue_Date_String,
                                tasTitle = it.tasTitle,

                            });
                        }

                    }

                if (!(string.IsNullOrEmpty(sortCulmn + "" + sortColumnDir)))
                {
                    taskModel = taskModel.OrderBy(sortCulmn + " " + sortColumnDir).ToList();
                }
                totalRecords = taskModel.Count();
                var data = taskModel.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return View();
            }
        }

        public async Task<ActionResult> Test (string date)
        {
            var item = Session["useDetails"] as Users;
            int month = 0;
            if (string.IsNullOrWhiteSpace(date))
            {
                month = 0;
            }
            else
            {
                month = Convert.ToInt32(date);
            }

            TasksModel task = new TasksModel();
            List<TasksModel> taskModel = new List<TasksModel>();
            taskModel = await rpVM.GetReports(item.useCompanyID, month);
            task = new TasksModel
            {
                Items = taskModel,
            };

            return View(task);
        }
    }
}