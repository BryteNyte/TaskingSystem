using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskingSystem.ViewModels;

namespace TaskingSystem.Controllers
{
    public class ImagesController : Controller
    {
        ImagesViewModel imgVM = new ImagesViewModel();
        // GET: Images
        public async Task<ActionResult> AddFiles()
        {
            HttpFileCollectionBase files = Request.Files;
            int id = Convert.ToInt32(Request["taskID"]);
            if (files.Count > 0)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string imageType = file.FileName;
                    imageType = imageType.Split('.').LastOrDefault().ToLower();
                    var type = ReturnExcludedExtension(imageType);
                    if (type == "true")
                    {
                        return Json(new { success = false, responseText = "The attached file is not supported." }, JsonRequestBehavior.AllowGet);
                    }
                    await imgVM.UploadImagesFromTaskDetails(id,file);
                }
                return Json(new { success = true, responseText = "Your files were sent successfully!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, responseText = "Please select files to be uploaded!" }, JsonRequestBehavior.AllowGet);
            }
           
           
        }

        public async Task DownloadAttachment(int id)
        {
           
            

          var path =  await imgVM.GetFilePath(id);
            var pt = ConfigurationManager.AppSettings["virtulUrl"].ToString() + path;
            // Create New instance of FileInfo class to get the properties of the file being downloaded
            FileInfo myfile = new FileInfo(pt);

            // Checking if file exists
            if (myfile.Exists)
            {
                // Clear the content of the response
                Response.ClearContent();

                // Add the file name and attachment, which will force the open/cancel/save dialog box to show, to the header
                Response.AddHeader("Content-Disposition", "attachment; filename=" + myfile.Name);

                // Add the file size into the response header
                Response.AddHeader("Content-Length", myfile.Length.ToString());

                // Set the ContentType
                Response.ContentType = ReturnExtension(myfile.Extension.ToLower());

                // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                Response.TransmitFile(myfile.FullName);

                // End the response
                Response.End();
            }
        }

        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }

        private string ReturnExcludedExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case "htm":
                case "html":
                case "log":
                case "tiff":
                case "tif":
                case "asf":
                case "avi":
                case "zip":
                case "wav":
                case "mp3":
                case "mpg":
                case "mpeg":
                case ".asp":
                case "fdf":
                case "exe":
                    return "true";
                default:
                    return "false";
            }
        }
    }
}