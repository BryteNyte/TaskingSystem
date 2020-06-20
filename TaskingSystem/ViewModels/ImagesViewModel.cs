using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TaskingSystem.Models;

namespace TaskingSystem.ViewModels
{
    public class ImagesViewModel
    {
        public async Task<bool> UploadImagesFromTaskDetails(int id,HttpPostedFileBase file)
        {
            try
            {
                ImagesModel img = new ImagesModel();
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(file.InputStream))
                {
                    bytes = br.ReadBytes(file.ContentLength);
                }

                img.imgImage_Name = file.FileName;
                img.imgImage_Image = bytes;
                img.imgImage_Type = file.ContentType;
                img.imgType = 2;
                img.imgID = id;
                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(img);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await httpClient.PostAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/AddFiles/", httpContent);
                if (response.IsSuccessStatusCode)
                {

                    return true;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<string> GetFilePath(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    ImagesModel img = new ImagesModel();
                    using (HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MySeverUrl"] + "/api/GetFilePath/id=" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var textresponse = await content.ReadAsStringAsync();
                                var json = JsonConvert.DeserializeObject<ImagesModel>(textresponse);

                                img.imgPath_To_Image = json.imgPath_To_Image;

                            }
                            return img.imgPath_To_Image;
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
    }
}