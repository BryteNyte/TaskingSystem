using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskingSystem.Models
{
    public class SupplierModel
    {
        public int supID { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string supName { get; set; }

        public int supUserID { get; set; }

        public int supUser_CompanyID { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string supEmail { get; set; }
        public string supContact { get; set; }
        [Required]
        [Display(Name = "Street Number")]
        public string streetNumber { get; set; }
        [Required]
        [Display(Name = "Street Name")]
        public string streetname { get; set; }
        [Required]
        [Display(Name = "City")]
        public string city { get; set; }
        [Required]
        [Display(Name = "Suburb")]
        public string suburb { get; set; }
        [Required]
        [Display(Name = "province")]
        public string province { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        public string code { get; set; }
        [Required]
        [Display(Name = "Contact Person Name")]
        public string supContactPersonName { get; set; }
        [Required]
        [Display(Name = "Contact Person Name")]
        public string supContactPersonNumber { get; set; }
        public string supAddress { get; set; }
        public string Action { get; set; }
        public int supActive { get; set; }
        public IEnumerable<SupplierModel> Items { get; set; }
        public string shortAddress { get; set; }
        public string _country { get; set; }
        public DateTime supDateAdded { get; set; }
        public string Date { get; set; }

    
    }
}