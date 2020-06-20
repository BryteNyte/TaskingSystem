using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using TaskingSystem.Models;

namespace TaskingSystem.ViewModels
{
    public class LoginViewModel
    {
        //public void CreateAuthCookie(string usrname)
        //{
        //    string usrData = "";
        //    FormsAuthenticationTicket authenticationTicket = new FormsAuthenticationTicket(
        //        1, usrname, DateTime.Now, DateTime.Now.AddDays(30), false, usrData, FormsAuthentication.FormsCookiePath);
        //    string encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);
        //    HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //    System.Web.HttpContext.Current.Response.Cookies.Add(authenticationCookie);
        //}

        public async Task<Users> LoginUser(Users login)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Users user = new Users();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/LoginUser/UserName="+ login.useUserName + "/" + "Password=" + login.usePassword + "/" ))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<Users>(textresponse);
                                user.useAccessLvl = json.useAccessLvl;
                                user.useCompanyID = json.useCompanyID;
                                user.useID = json.useID;
                                user.useName = json.useName + " " + json.useSurname;
                                user.useImageName = json.useImageName;
                                user.useCompanyName = json.useCompanyName;
                                //FormsAuthentication.SetAuthCookie(user.useName, false);
                            }
                            return user;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        //public void Logout()
        //{
        
        //        FormsAuthentication.SignOut();
        
        //}
    }
}