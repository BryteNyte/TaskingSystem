using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskingSystem.Models
{
    public class TypeModel
    {
        public int typID { get; set; }
        [Required]
        [Display(Name = "Type Name")]
        public string typName { get; set; }
        public string typDate_Created { get; set; }
        public string Action { get; set; }
    }
}