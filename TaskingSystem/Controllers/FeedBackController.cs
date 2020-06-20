using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskingSystem.Models;
using TaskingSystem.ViewModels;

namespace TaskingSystem.Controllers
{
 
    public class FeedBackController : Controller
    {
        FeedbackViewModel feedVM = new FeedbackViewModel();
        // GET: FeedBack
        public async Task<ActionResult> MessagesList(int? taskID)
        {
            List<FeedbackModel> lstVm = new List<FeedbackModel>();
            if (taskID > 0)
            {
                lstVm = await feedVM.Get(taskID);
            }
            else
            {
                var id = Convert.ToInt32(this.Session["taskID"]);
                lstVm = await feedVM.Get(id);
            }

           
         
            FeedbackModel msgModel = new FeedbackModel();
           
            if (lstVm != null)
            {

                msgModel = new FeedbackModel
                {

                    Items = lstVm,

                };


                return View(msgModel);
            }
            else
            {
                msgModel = new FeedbackModel
                {
                    Items = null,

                };

                return View(msgModel);
            }
            
        }

        public async Task<ActionResult> MessagesPost(int taskID,string msg, string taken,string required,int type,bool bill)
        {
            if (Session["useDetails"] != null)
            {
              
                int result = 0;
                int timeTaken = 0;
                int timeRequired = 0;
                if (!string.IsNullOrWhiteSpace(taken))
                {
                    if (int.TryParse(taken, out result))
                    {


                        timeTaken = Convert.ToInt32(taken);

                    }
                    else
                    {
                        return Json(new { success = false, responseText = "Please only input numbers in Time taken field!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {

                }
                if (!string.IsNullOrWhiteSpace(required))
                {
                    if (int.TryParse(required, out result))
                    {

                        timeRequired = Convert.ToInt32(required);


                    }
                    else
                    {
                        return Json(new { success = false, responseText = "Please only input numbers in Time Required field!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    
                }



                if (string.IsNullOrWhiteSpace(msg))
                {
                    return Json(new { success = false, responseText = "Feedback field cannot be empty!" }, JsonRequestBehavior.AllowGet);
                }

                var userID = this.Session["useDetails"] as Users;
                FeedbackModel feedback = new FeedbackModel();
                feedback.fdbCreatedBy = userID.useID;
                feedback.fdbFeedBack = msg;
                feedback.fdbTaskID = taskID;
                feedback.fdbTimeTaken = timeTaken;
                feedback.fdbRequiredTime = timeRequired;
                feedback.fdbStatusType = type;
                 feedback.fdbCheckBox = bill;
            
               
                var response = await feedVM.AddFeedBack(feedback);

                //List<FeedbackModel> lstVm = await feedVM.Get(taskID);
                //FeedbackModel msgModel = new FeedbackModel();

                if (response)
                {

                    //msgModel = new FeedbackModel
                    //{

                    //    Items = lstVm,

                    //};


                    return Json(new { success = true, responseText = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //msgModel = new FeedbackModel
                    //{
                    //    Items = null,

                    //};

                    return Json(new { success = false, responseText = "An error has occured" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("../Login/Login");
            }
            
            
        }
    }
}