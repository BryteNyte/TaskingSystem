using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TaskingSystem.Models;

namespace TaskingSystem.ViewModels
{
    public class FeedbackViewModel
    {
        public async Task<List<FeedbackModel>> Get(int? ID)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    List<FeedbackModel> fdb = new List<FeedbackModel>();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetFeedBack/ID="+ ID))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<List<FeedbackModel>>(textresponse);
                                foreach (var i in json)
                                {
                                    fdb.Add(new FeedbackModel
                                    {
                                        fdbFeedBack = i.fdbFeedBack,
                                        fdbRequiredTime = i.fdbRequiredTime,
                                        fdbTimeTaken = i.fdbTimeTaken,
                                        fdbDateCreated = i.fdbDateCreated,
                                        fdbCreatedBy_Name = i.fdbCreatedBy_Name,
                                        fdbStatusType = i.fdbStatusType,
                                        fdbCreatedBy = i.fdbCreatedBy,
                                    });
                                }
                            }
                            return fdb;
                        }
                        else
                        {
                            return fdb;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> AddFeedBack(FeedbackModel feed)
        {
            try
            {

                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(feed);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/AddFeedBack/", httpContent);
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
    }
}