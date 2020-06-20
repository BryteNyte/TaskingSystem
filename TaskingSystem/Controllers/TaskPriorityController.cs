using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskingSystem.Models;
using System.Linq.Dynamic;
using TaskingSystem.ViewModels;

namespace TaskingSystem.Controllers
{
    
    public class TaskPriorityController : Controller
    {
        TaskPriorityViewModel prtVM = new TaskPriorityViewModel();
        // GET: TaskPriority
        public ActionResult Priority_Details()
        {
            return View();
        }

        public async Task<ActionResult> GetPriority()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortCulmn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;
            var typName = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            List<PriorityModel> typ = new List<PriorityModel>();
            typ = await prtVM.GetTaskPriority();
            if (!string.IsNullOrEmpty(typName))
            {
                typ = typ.Where(a => a.ptyName.ToLower().Contains(typName.ToLower())).ToList();
            }
            if (!(string.IsNullOrEmpty(sortCulmn + "" + sortColumnDir)))
            {
                typ = typ.OrderBy(sortCulmn + " " + sortColumnDir).ToList();
            }
            totalRecords = typ.Count();
            var data = typ.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddPriority()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddPriority(PriorityModel typ)
        {
            if (ModelState.IsValid == false)
            {
                return View(typ);
            }
            else
            {
                var response = await prtVM.AddPriority(typ);
                if (response == true)
                {
                    return RedirectToAction("Priority_Details");
                }
                else
                {
                    return View(typ);
                }

            }

        }

        [HttpGet]
        public async Task<ActionResult> UpdatePriority(int id)
        {
            PriorityModel typ = new PriorityModel();
            try
            {
                typ = await prtVM.UpdateGet(id);
            }
            catch (Exception)
            {
                //TempData["Message"] = "Unable to process request !";
                return View(typ);
            }

            return View(typ);

        }

        [HttpPost]
        public async Task<ActionResult> UpdatePriority(PriorityModel typ)
        {

            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(typ);
                }
                else
                {
                    await prtVM.UpdatePriority(typ);
                }
                return RedirectToAction("Priority_Details");
            }
            catch (Exception e)
            {
                //TempData["Message"] = "Unable to process request !";
                return View(typ);
            }


        }


        public async Task<ActionResult> DeletePriority(int id)
        {
            PriorityModel tModel = new PriorityModel();
            tModel.ptyID = id;
            await prtVM.DeletePriority(tModel);
            return RedirectToAction("Priority_Details");


        }
    }
}