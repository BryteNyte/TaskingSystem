using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskingSystem.Models
{
    public class CompanyModel
    {
        public CompanyModel()
        {
            this.Companies = new List<SelectListItem>();
        }
        public List<SelectListItem> Companies { get; set; }

        public int cmpID { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string cmpName { get; set; }
        [Required]
        [Display(Name = "Registration Number")]
        public string cmpRegNumber { get; set; }
        [Required]
        [Display(Name = "Vat Number")]
        public string cmpVatNumber { get; set; }
        [Required]
        [Display(Name = "Email Address")]
        public string cmpEmail { get; set; }
       
        public string cmpContact { get; set; }
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
        public string cmpContactPersonName { get; set; }
        [Required]
        [Display(Name = "Contact Person Name")]
        public string cmpContactPersonNumber { get; set; }

        public string cmpImageName { get; set; }
        public byte[] cmpImageFile { get; set; }
        
        public string cmpAddress { get; set; }
       
        public string Action { get; set; }
        public int cmpActive { get; set; }
        public IEnumerable<CompanyModel> Items { get; set; }
        public List<CompanyModel> _companies { get; set; }
        public List<CompanyConnectionModel> _connections { get; set; }
        public string shortAddress { get; set; }

        public string _country { get; set; }
        public DateTime cmpDateAdded { get; set; }
        public string Date { get; set; }

        public int cndRemoved { get; set; }
        public int cndAccepted { get; set; }
        public int cndPending { get; set; }
        public int cndActionID { get; set; }
        public int cndStatus { get; set; }
    }
}