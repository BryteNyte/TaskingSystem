using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskingSystem.Models;
using TaskingSystem.ViewModels;
using System.Linq.Dynamic;
using System.Data;
using TaskingSystem.Context;
using System.Threading.Tasks;
using System.IO;

namespace TaskingSystem.Controllers
{
 
    public class TasksController : Controller
    {
        TasksViewModel taskVm = new TasksViewModel();
        CompanyConnectionViewModel cnnVM = new CompanyConnectionViewModel();
        UsersViewModel usVM = new UsersViewModel();
        // GET: Tasks

        public async Task<ActionResult> Tasks()
        {
            var cmpID = 0;
            Users item = new Users();
            if (Session["useDetails"] != null)
            {
                item = Session["useDetails"] as Users;
            }

            TasksModel model = new TasksModel();
            
          
            cmpID = item.useCompanyID;
            List<TypeModel> typModel = new List<TypeModel>();
            List<Users> usrModel = new List<Users>();
            List<Users> toModel = new List<Users>();
            List<PriorityModel> ptyModel = new List<PriorityModel>();
            List<StatusModel> staModel = new List<StatusModel>();
            UserContext useContext = new UserContext();
            TypeContext typContext = new TypeContext();
            PriorityContext ptyContext = new PriorityContext();
            StatusContext staContext = new StatusContext();
            usrModel = await usVM.GetUserConnection(cmpID);
            //toModel = useContext.ToUsersDropDown(cmpID);
            typModel = typContext.TypeDropDown();
            ptyModel = ptyContext.PriorityDropDown();
            staModel = staContext.StatusDropDown();
            foreach (var user in usrModel)
            {
                model.Users.Add(new SelectListItem { Text = user.useName, Value = user.useID.ToString() });
            }
            //foreach (var user in toModel)
            //{
            //    model.ToUsers.Add(new SelectListItem { Text = user.useName, Value = user.useID.ToString() });
            //}
            foreach (var type in typModel)
            {
                model.Types.Add(new SelectListItem { Text = type.typName, Value = type.typID.ToString() });
            }
            foreach (var priority in ptyModel)
            {
                model.Priorites.Add(new SelectListItem { Text = priority.ptyName, Value = priority.ptyID.ToString() });
            }
            foreach (var status in staModel)
            {
                model.Status.Add(new SelectListItem { Text = status.staName, Value = status.staID.ToString() });
            }
            return View(model);
        

        }

        public async Task<ActionResult> GetTasks()
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
            var type = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Name = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var from = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var to = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var priority = Request.Form.GetValues("columns[7][search][value]").FirstOrDefault();
            var status = Request.Form.GetValues("columns[8][search][value]").FirstOrDefault();
            var startDate = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();
            var endDate = Request.Form.GetValues("columns[10][search][value]").FirstOrDefault();
            List<TasksModel> taskModel = new List<TasksModel>();
            taskModel = await taskVm.GetTasks(useID);
            if (!string.IsNullOrWhiteSpace(Name))
            {
                taskModel = taskModel.Where(a => a.tasTitle.ToLower().Contains(Name.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(from))
            {
                taskModel = taskModel.Where(a => a.tasDelagatedBy == Convert.ToInt32(from)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(to))
            {
                taskModel = taskModel.Where(a => a.tasDelegatedTo == Convert.ToInt32(to)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                taskModel = taskModel.Where(a => a.tasType_Name == type).ToList();
            }
            if (!string.IsNullOrWhiteSpace(priority))
            {
                taskModel = taskModel.Where(a => a.tasPriority_Name == priority).ToList();
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                taskModel = taskModel.Where(a => a.statusName == status).ToList();
            }
            if (!string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                taskModel = taskModel.Where(a => Convert.ToDateTime(a.tasDue_Date_String) == Convert.ToDateTime(startDate)).ToList();
            }
            if (string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                taskModel = taskModel.Where(a => Convert.ToDateTime(a.tasDue_Date_String) == Convert.ToDateTime(endDate)).ToList();
            }
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                taskModel = taskModel.Where(a => Convert.ToDateTime(a.tasDue_Date_String) >= Convert.ToDateTime(startDate) && Convert.ToDateTime(a.tasDue_Date_String) <= Convert.ToDateTime(endDate)).ToList();
            }
            if (!(string.IsNullOrEmpty(sortCulmn + "" + sortColumnDir)))
            {
                taskModel = taskModel.OrderBy(sortCulmn + " " + sortColumnDir).ToList();
            }
            totalRecords = taskModel.Count();
            var data = taskModel.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> AddTask()
        {
            var useID = 0;
            var cmpID = 0;
            var cmpName = "";
            Users item;
            if (Session["useDetails"] != null)
            {
                item = Session["useDetails"] as Users;
                useID = item.useID;
                cmpID = item.useCompanyID;
                cmpName = item.useCompanyName;
            }
           
       
            TasksModel model = new TasksModel();
            List<CompanyModel> cmpModel = new List<CompanyModel>();
            List<TypeModel> typModel = new List<TypeModel>();
            List<PriorityModel> ptyModel = new List<PriorityModel>();
            List<Users> usrModel = new List<Users>();
            CompanyContext cmpContext = new CompanyContext();
            TypeContext typContext = new TypeContext();
            PriorityContext ptyContext = new PriorityContext();
            UserContext useContext = new UserContext();
            cmpModel = await cnnVM.CompanyConnectionDropDown(cmpID, cmpName);
            typModel = typContext.TypeDropDown();
            ptyModel = ptyContext.PriorityDropDown();

            usrModel = useContext.UsersDropDownController(cmpID);
            model.tasID = cmpID;
            foreach (var company in cmpModel)
            {
                model.Companies.Add(new SelectListItem { Text = company.cmpName, Value = company.cmpID.ToString() });
            }
            foreach (var type in typModel)
            {
                model.Types.Add(new SelectListItem { Text = type.typName, Value = type.typID.ToString() });
            }
            foreach (var priority in ptyModel)
            {
                model.Priorites.Add(new SelectListItem { Text = priority.ptyName, Value = priority.ptyID.ToString() });
            }
            foreach (var user in usrModel)
            {
                model.Users.Add(new SelectListItem { Text = user.useName, Value = user.useID.ToString() });
            }
            model.tasDelagatedBy = useID;

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> AddTask(TasksModel tasks, HttpPostedFileBase[] postedFile)
        {
         
            var useID = 0;
            var cmpID = 0;
            var cmpName = "";
                var item = Session["useDetails"] as Users;
                useID = item.useID;
                cmpID = item.useCompanyID;
            cmpName = item.useCompanyName;
            TasksModel model = new TasksModel();
            List<CompanyModel> cmpModel = new List<CompanyModel>();
            List<TypeModel> typModel = new List<TypeModel>();
            List<PriorityModel> ptyModel = new List<PriorityModel>();
            List<Users> usrModel = new List<Users>();
            TypeContext typContext = new TypeContext();
            CompanyContext cmpContext = new CompanyContext();
            PriorityContext ptyContext = new PriorityContext();
            UserContext useContext = new UserContext();
            cmpModel = await cnnVM.CompanyConnectionDropDown(cmpID,cmpName);
            typModel = typContext.TypeDropDown();
            ptyModel = ptyContext.PriorityDropDown();
            usrModel = useContext.UsersDropDownController(cmpID);
            foreach (var country in cmpModel)
            {
                model.Companies.Add(new SelectListItem { Text = country.cmpName, Value = country.cmpID.ToString() });
            }
            foreach (var type in typModel)
            {
                model.Types.Add(new SelectListItem { Text = type.typName, Value = type.typID.ToString() });
            }
            foreach (var priority in ptyModel)
            {
                model.Priorites.Add(new SelectListItem { Text = priority.ptyName, Value = priority.ptyID.ToString() });
            }
            foreach (var user in usrModel)
            {
                model.Users.Add(new SelectListItem { Text = user.useName, Value = user.useID.ToString() });
            }
            model.tasDelagatedBy = tasks.tasDelagatedBy;
            model.tasDelegatedTo = tasks.tasDelegatedTo;
            //if (tasks.tasType > 0 && !string.IsNullOrWhiteSpace(tasks.tasTitle))
            //{
            //    if (ModelState.IsValid == false)
            //    {
            //        return View(model);
            //    }
            //    else
            //    {
            //        var response = await taskVm.AddTask(tasks, postedFile);
            //        if (response == true)
            //        {
            //            return RedirectToAction("Tasks");
            //        }
            //        else
            //        {
            //            return View(model);
            //        }

            //    }
            //}
            //else
            //{
            //    ViewBag.CompanyID = tasks.tasDelegatedTo_Company;
            //    return View(model);
            //}
            ViewBag.CompanyID = tasks.tasDelegatedTo_Company;
            return View(model);
        }
       
        public async Task<ActionResult> CreateTask()
        {
            try
            {

                TasksModel task = new TasksModel();
                List<ImagesModel> img = new List<ImagesModel>();
                HttpFileCollectionBase files = Request.Files;
                int taskTo = Convert.ToInt32(Request["taskTo"]) ;
                int taskFrom = Convert.ToInt32(Request["taskFrom"]);
                DateTime date = Convert.ToDateTime(Request["date"]);
                int type = Convert.ToInt32(Request["type"]);
                int priority = Convert.ToInt32(Request["priority"]);
                string title = Request["title"].ToString();
                string desc = Request.Unvalidated["desc"].ToString();

                task.tasDelegatedTo = taskTo;
                task.tasDelagatedBy = taskFrom;
                task.tasDue_Date = date;
                task.tasType = type;
                task.tasPriority = priority;
                task.tasTitle = title;
                task.Description = desc;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        byte[] bytes;

                        using (BinaryReader br = new BinaryReader(files[i].InputStream))
                        {
                            bytes = br.ReadBytes(files[i].ContentLength);
                        }
                        img.Add(new ImagesModel
                        {
                            imgImage_Name = files[i].FileName,
                            imgImage_Type = files[i].ContentType,
                            imgImage_Image = bytes,
                            imgType = 2,
                        });
                    }
                    task.Images = img;
                }
                var response = await taskVm.AddTask(task);
                if (response)
                {
                    return Json(new { success = true, responseText = "Yay!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = "Ahhh!" }, JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception)
            {

                return Json(new { success = false, responseText = "Please Complete all required (*) form fields!" }, JsonRequestBehavior.AllowGet);
            }
           

        }

        public async Task<ActionResult> UserDropDown(int cmpID)
        {
            TasksModel model = new TasksModel();
            List<Users> usrModel = new List<Users>();
            UserContext useContext = new UserContext();
            usrModel = useContext.UsersDropDownController(cmpID);
            foreach (var user in usrModel)
            {
                model.Users.Add(new SelectListItem { Text = user.useName, Value = user.useID.ToString() });
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> TaskDetails(int id)
        {
            TasksModel tsk = new TasksModel();
            try
            {
                int useID = 0;
                Users item;
                if (Session["useDetails"] != null)
                {
                    item = Session["useDetails"] as Users;
                    useID = item.useID;
                }
               
             
                ViewBag.UserID = useID;
                tsk = await taskVm.TaskDetails(id);
                //tsk.Images = await taskVm.GetImagesForTask(id);

            }
            catch (Exception)
            {
                //TempData["Message"] = "Unable to process request !";

            }

            return View(tsk);

        }

        public async Task<ActionResult> PartialImages(int taskID)
        {
            TasksModel tsk = new TasksModel();
            tsk.Images = await taskVm.GetImagesForTask(taskID);
            return View(tsk);
        }

        public async Task<ActionResult> GetNumber(int taskID)
        {
            var response = await taskVm.TaskDetails(taskID);
            return View(response);
        }

        public async Task<ActionResult> GetStatus(int taskID)
        {
            TasksModel ts = new TasksModel();
            var response = await taskVm.TaskDetails(taskID);
            return View(response);

        }

        public async Task<ActionResult> GetAssignedTo(int taskID)
        {
            var response = await taskVm.TaskDetails(taskID);
            return View(response);
        }

        public async Task<ActionResult> GetDate(int taskID)
        {
            var response = await taskVm.GetDate(taskID);
            TasksModel tm = new TasksModel();
            tm.tasDue_Date_String = response;
            return View(tm);
        }

        public async Task<ActionResult> GetTimes(int taskID)
        {
            var takenResponse = await taskVm.GetTimeTaken(taskID);
            var requiredResponse = await taskVm.GetRequiredTime(taskID);
            FeedbackModel fd = new FeedbackModel();
            fd.fdbTimeTaken = takenResponse;
            fd.fdbRequiredTime = requiredResponse;
            return View(fd);
        }

        public async Task<ActionResult> UpdateOrderNumber(int taskID, string number)
        {
            await taskVm.UpdateOrderNumber(taskID, number);
            return Json(new { success = true, responseText = "Please select files to be uploaded!" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdateDate(int taskID, DateTime date)
        {
            await taskVm.UpdateDate(taskID, date);
            return Json(new { success = true, responseText = "Success!" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdateUser(int taskID, int user)
        {
            await taskVm.UpdateUser(taskID, user);
            return Json(new { success = true, responseText = "Task Assigned to changed successfully!" }, JsonRequestBehavior.AllowGet);
        }

     
    }
}