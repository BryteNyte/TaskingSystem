using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskingSystem.Models;

namespace TaskingSystem.ViewModels
{
    public class SuppliersViewModel
    {
        UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        public async Task<bool> AddSupplier(SupplierModel sup)
        {
            try
            {

                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(sup);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/AddSupplier/", httpContent);
                if (response.IsSuccessStatusCode)
                {

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                return false;
            }

        }

        public async Task<List<SupplierModel>> Get(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    List<SupplierModel> cmp = new List<SupplierModel>();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetSuppliers/id=" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<List<SupplierModel>>(textresponse);
                                foreach (var i in json)
                                {
                                    cmp.Add(new SupplierModel
                                    {
                                        supID = i.supID,
                                        supName = i.supName,
                                       
                                        supAddress = i.streetNumber + " " + i.streetname + " " + i.suburb + " " + i.city + " " + i.province + " " + i.code,
                                        supEmail = i.supEmail,
                                        Date = i.Date,
                                        supContactPersonName = i.supContactPersonName,
                                        supContactPersonNumber = i.supContactPersonNumber,
                                        shortAddress = i.streetNumber + " " + i.streetname + " " + i.suburb + " " + i.city,
                                        Action = "<a class='btn btn-info btn-xs' style='margin-top:6px'  href='" + helper.Action("Supplier_Details", "Suppliers", new { Id = i.supID }) + "'>View</a>" + "<a class='btn btn-danger btn-xs' style='margin-top:6px' onclick=' return confirm(\"Are you sure you wish to delete this record?\")' href='" + helper.Action("DeletePriority", "TaskPriority", new { Id = i.supID }) + "'>Delete</a>"

                                    });
                                }
                            }
                            return cmp;
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

        public async Task<SupplierModel> SupplierDetails(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    SupplierModel sup = new SupplierModel();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/SupplierDetails/id=" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<SupplierModel>(textresponse);

                                sup.supName = json.supName;
                                sup.supID = json.supID;
                                sup.supAddress = json.streetNumber + " " + json.streetname + " " + json.suburb + " " + json.city + " " + json.province + " " + json.code;
                                sup.supContactPersonName = json.supContactPersonName;
                                sup.supContactPersonNumber = json.supContactPersonNumber;
                                sup.supEmail = json.supEmail;

                            }
                            return sup;
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

        public async Task<bool> UpdatePriority(PriorityModel typ)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(typ);
                    using (HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        HttpResponseMessage response = null;
                        response = await httpClient.PutAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/UpdatePriority/", httpContent);
                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> DeletePriority(PriorityModel typ)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(typ);
                    using (HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        HttpResponseMessage response = null;
                        response = await httpClient.PutAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/DeletePriority/", httpContent);
                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}