using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskingSystem.Models
{
    public class TasksModel
    {
        public  TasksModel() {
            this.Companies = new List<SelectListItem>();
            this.Users = new List<SelectListItem>();
            this.Types = new List<SelectListItem>();
            this.Priorites = new List<SelectListItem>();
            this.ToUsers = new List<SelectListItem>();
            this.Status = new List<SelectListItem>();
            this.DateList = new List<SelectListItem>();
            
        }

        public List<SelectListItem> DateList { get; set; }
        public List<SelectListItem> Companies { get; set; }
        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> Types { get; set; }
        public List<SelectListItem> Priorites { get; set; }
        public List<SelectListItem> ToUsers { get; set; }
        public List<SelectListItem> Status { get; set; }
        public int tasID { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string tasTitle { get; set; }
        public string tasDescription { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public int tasDelagatedBy { get; set; }
        
        public int tasDelegatedTo { get; set; }
        public int tasDeledatedBy_Company { get; set; }
        [Required]
        [Display(Name = "Company delegated to")]
        public int tasDelegatedTo_Company { get; set; }
        public string tasDelegatedBy_Name { get; set; }
        public string tasDelegatedTo_Name { get; set; }
        public string tasDelegatedBy_Company_Name { get; set; }
        public string tasDelegatedTo_Company_Name { get; set; }
        public int tasType { get; set; }
        public string tasType_Name { get; set; }
        public string tasDue_Date_String { get; set; }
        public string tasCreated_Date { get; set; }
        public DateTime tasDue_Date { get; set; }
        public int tasPriority { get; set; }
        public string tasPriority_Name { get; set; }
        public int tasStatus { get; set; }
        public string tasStatus_Name { get; set; }
        public string Action { get; set; }
        public string feed { get; set; }
        public string tasOrderNumber { get; set; }
        public string statusName { get; set; }
        public int tasWorked { get; set; }
        public int tasTimeSpent { get; set; }
        public int tasTimeRequired { get; set; }
        public int tasBillable { get; set; }
        public List<ImagesModel> Images { get; set; }
        public IEnumerable<TasksModel> Items { get; set; }



    }
}