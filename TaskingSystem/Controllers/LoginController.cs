using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskingSystem.Models;
using TaskingSystem.ViewModels;

namespace TaskingSystem.Controllers
{
    
    public class LoginController : Controller
    {
        LoginViewModel lgVm = new LoginViewModel();
        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            Users lgModel = new Users();
            return View(lgModel);
        }

        [HttpPost]
        public async Task<ActionResult> Login(Users login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            login = await lgVm.LoginUser(login);
            if (login != null)
            {
                this.Session["useDetails"] = login;

                return Redirect("~/Tasks/Tasks");
            }
            else
            {
                ViewBag.Message = "Incorrect Username Or Password";
                return View(login);
            }
        }

        public ActionResult LogOut()
        {
            
            if (this.Session["useDetails"] != null)
            {
                this.Session.Clear();
                //lgVm.Logout();
            }
            return View("Login");
        }
    }
}