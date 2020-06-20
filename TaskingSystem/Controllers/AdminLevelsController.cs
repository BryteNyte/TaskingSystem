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
   
    public class AdminLevelsController : Controller
    {
        // GET: AdminLevels
        AdminLevelsViewModel admVM = new AdminLevelsViewModel();
        // GET: TaskPriority
        public ActionResult Admin_Details()
        {
            return View();
        }

        public async Task<ActionResult> GetAdmin()
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
            List<AdminLevelModel> adm = new List<AdminLevelModel>();
            adm = await admVM.Get();
            if (!string.IsNullOrEmpty(Name))
            {
                adm = adm.Where(a => a.admName.ToLower().Contains(Name.ToLower())).ToList();
            }
            if (!(string.IsNullOrEmpty(sortCulmn + "" + sortColumnDir)))
            {
                adm = adm.OrderBy(sortCulmn + " " + sortColumnDir).ToList();
            }
            totalRecords = adm.Count();
            var data = adm.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
        {
            AdminLevelModel model = new AdminLevelModel();
            var items = 9;
            for (int i = 1; i < items; i++)
            {
                model.Levels.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            


            return View(model);
        
        }

        [HttpPost]
        public async Task<ActionResult> Add(AdminLevelModel adm)
        {
            if (ModelState.IsValid == false)
            {
                return View(adm);
            }
            else
            {
                var response = await admVM.Add(adm);
                if (response == true)
                {
                    return RedirectToAction("Admin_Details");
                }
                else
                {
                    return View(adm);
                }

            }

        }

        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            AdminLevelModel adm = new AdminLevelModel();
            var items = 9;
            
            try
            {
                adm = await admVM.UpdateGet(id);
                for (int i = 1; i < items; i++)
                {
                    adm.Levels.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                }
            }
            catch (Exception)
            {
                //TempData["Message"] = "Unable to process request !";
                return View(adm);
            }

            return View(adm);

        }

        [HttpPost]
        public async Task<ActionResult> Update(AdminLevelModel adm)
        {

            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(adm);
                }
                else
                {
                    await admVM.Update(adm);
                }
                return RedirectToAction("Admin_Details");
            }
            catch (Exception e)
            {
                //TempData["Message"] = "Unable to process request !";
                return View(adm);
            }


        }


        public async Task<ActionResult> Delete(int id)
        {
            AdminLevelModel tModel = new AdminLevelModel();
            tModel.admID = id;
            await admVM.Delete(tModel);
            return RedirectToAction("Admin_Details");


        }
    }
}