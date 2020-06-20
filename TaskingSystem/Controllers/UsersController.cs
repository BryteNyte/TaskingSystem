using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskingSystem.Models;
using TaskingSystem.ViewModels;
using System.Linq.Dynamic;

namespace TaskingSystem.Controllers
{
   
    public class UsersController : Controller
    {
        UsersViewModel usrVM = new UsersViewModel();
        // GET: Users
        public ActionResult User_Details()
        {
            if (Session["useDetails"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Login/Login");
            }
           
        }


        public async Task<ActionResult> GetUsers()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortCulmn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;
            var Name = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            List<Users> usr = new List<Users>();
            usr = await usrVM.Get();
            if (!string.IsNullOrEmpty(Name))
            {
                usr = usr.Where(a => a.useName.ToLower().Contains(Name.ToLower())).ToList();
            }
            if (!(string.IsNullOrEmpty(sortCulmn + "" + sortColumnDir)))
            {
                usr = usr.OrderBy(sortCulmn + " " + sortColumnDir).ToList();
            }
            totalRecords = usr.Count();
            var data = usr.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Add(Users usr,HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid == false)
            {
                return View(usr);
            }
            else
            {
                var response = await usrVM.Add(usr, postedFile);
                if (response == false)
                {
                    return View(usr);
                }
            }
            return RedirectToAction("User_Details");
        }


    }
}