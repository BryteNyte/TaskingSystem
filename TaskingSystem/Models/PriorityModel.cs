using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskingSystem.Models
{
    public class PriorityModel
    {

        public int ptyID { get; set; }
        [Required]
        [Display(Name = "Priority Name")]
        public string ptyName { get; set; }
        public string ptyDate_Created { get; set; }
        public string Action { get; set; }
    }
}