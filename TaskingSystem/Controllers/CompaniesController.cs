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
    public class CompaniesController : Controller
    {
        AdminCompanyViewModel cmpVM = new AdminCompanyViewModel();
        CompanyConnectionViewModel conVM = new CompanyConnectionViewModel();
        // GET: Companies
        public async Task<ActionResult> CompanyConnections()
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

        public async Task<ActionResult> ReturnCompanyConnections(string search)
        {
            if (Session["useDetails"] != null)
            {
                var cmpID = Session["useDetails"] as Users;
                CompanyModel viewModel;
                List<CompanyModel> con = new List<CompanyModel>();
                con = await conVM.GetConnection(cmpID.useCompanyID);
                con = con.OrderBy(a => a.cmpName).ToList();
                if (!string.IsNullOrWhiteSpace(search))
                {
                    con = con.Where(c => c.cmpName.ToLower().Contains(search.ToLower())).ToList();
                }
                viewModel = new CompanyModel
                {
                    _companies = con,

                };
               

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("../Login/Login");
            }
        }

        public async Task<ActionResult> AddConnection(int requestedCmpID)
        {
            var item = Session["useDetails"] as Users;
            await conVM.AddConnection(requestedCmpID, item.useCompanyID);
            return Json(new { success = true, responseText = "Success!" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AcceptConnection(int requestedCmpID)
        {
            var item = Session["useDetails"] as Users;
            await conVM.AcceptConnection(requestedCmpID, item.useCompanyID);
            return Json(new { success = true, responseText = "Success!" }, JsonRequestBehavior.AllowGet);
        }


    }
}