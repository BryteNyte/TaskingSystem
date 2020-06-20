using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskingSystem.Models
{
    public class Users
    {
     
        public int useID { get; set; }
        public string useName { get; set; }
        public string useSurname { get; set; }
        public string useEmail { get; set; }
        public string useContact { get; set; }
        public int useCompanyID { get; set; }
        public string useCompanyName { get; set; }
        public int useAccessLvl { get; set; }
        public string useAccessLvlName { get; set; }
        public string usePassword { get; set; }
        public byte[] useImageFile { get; set; }
        public string useImageName { get; set; }
        public int useActive { get; set; }
        public DateTime useDateCreated { get; set; }
        public string Date { get; set; }
        public string Action { get; set; }
        public string useUserName { get; set; }

        public ImagesModel Images { get; set; }




    }
}