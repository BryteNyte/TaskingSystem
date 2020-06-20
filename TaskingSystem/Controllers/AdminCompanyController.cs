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
   
    public class AdminCompanyController : Controller
    {
        AdminCompanyViewModel cmpVM = new AdminCompanyViewModel();
        // GET: AdminCompany
        public ActionResult AdminCompany_Details()
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

        public async Task<ActionResult> GetAdminCompany()
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
            List<CompanyModel> cmp = new List<CompanyModel>();
            cmp = await cmpVM.Get();
            if (!string.IsNullOrEmpty(Name))
            {
                cmp = cmp.Where(a => a.cmpName.ToLower().Contains(Name.ToLower())).ToList();
            }
            if (!(string.IsNullOrEmpty(sortCulmn + "" + sortColumnDir)))
            {
                cmp = cmp.OrderBy(sortCulmn + " " + sortColumnDir).ToList();
            }
            totalRecords = cmp.Count();
            var data = cmp.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
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

        [HttpPost]
        public async Task<ActionResult> Add(CompanyModel cmp)
        {
            if (ModelState.IsValid == false)
            {
                return View(cmp);
            }
            else
            {
                var response = await cmpVM.AddCompany(cmp);
                if (response == true)
                {
                    return RedirectToAction("AdminCompany_Details");
                }
                else
                {
                    return View(cmp);
                }

            }
        }
    }
}