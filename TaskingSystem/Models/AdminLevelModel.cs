using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskingSystem.Models
{
    public class AdminLevelModel
    {

        public AdminLevelModel()
        {
            this.Levels = new List<SelectListItem>();
           
        }

        public List<SelectListItem> Levels { get; set; }
        public int admID { get; set; }
        [Required]
        [Display(Name = "Admin Level Name")]
        public string admName { get; set; }
        public string admDate_Created { get; set; }
        public int admLevel { get; set; }
        public string Action { get; set; }
    }
}