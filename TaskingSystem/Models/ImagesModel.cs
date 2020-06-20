using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskingSystem.Models
{
    public class ImagesModel
    {
        public int imgID { get; set; }
        public string imgImage_Name { get; set; }
        public string imgImage_Type { get; set; }
        public byte[] imgImage_Image { get; set; }
        public string imgPath_To_Image { get; set; }
        public int imgType { get; set; }
        public string imgDownloadPath { get; set; }
    }
}