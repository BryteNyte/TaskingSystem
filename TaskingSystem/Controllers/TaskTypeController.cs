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
    
    public class TaskTypeController : Controller
    {
        TaskTypeViewModel typVM = new TaskTypeViewModel();
        // GET: TaskType
     
        public ActionResult Type_Details()
        {
            return View();
        }

        public async Task<ActionResult> GetType()
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
            List<TypeModel> typ = new List<TypeModel>();
            typ = await typVM.GetTaskType();
            if (!string.IsNullOrEmpty(typName))
            {
                typ = typ.Where(a => a.typName.ToLower().Contains(typName.ToLower())).ToList();
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
        public ActionResult AddType()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddType(TypeModel typ)
        {
            if (ModelState.IsValid == false)
            {
                return View(typ);
            }
            else
            {
                var response = await typVM.AddType(typ);
                if (response == true)
                {
                    return RedirectToAction("Type_Details");
                }
                else
                {
                    return View(typ);
                }
                
            }
            
        }

        [HttpGet]
        public async Task<ActionResult> UpdateType(int id)
        {
            TypeModel typ = new TypeModel();
            try
            {
                typ = await typVM.UpdateGet(id);
            }
            catch (Exception)
            {
                //TempData["Message"] = "Unable to process request !";
                return View(typ);
            }

            return View(typ);
          
        }

        [HttpPost]
        public async Task<ActionResult> UpdateType(TypeModel typ)
        {

            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(typ);
                }
                else
                {
                    await typVM.UpdateType(typ);
                }
                return RedirectToAction("Type_Details");
            }
            catch (Exception e)
            {
                //TempData["Message"] = "Unable to process request !";
                return View(typ);
            }
           
          
        }

    
        public async Task<ActionResult> DeleteType(int id)
        {
            TypeModel tModel = new TypeModel();
            tModel.typID = id;
           await typVM.DeleteType(tModel);
            return RedirectToAction("Type_Details");


        }
    }
}