using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskingSystem.Models
{
    public class CompanyConnectionModel
    {
        public int cndID { get; set; }

        public int cndCmpId_one { get; set; }
        public int cndCmpId_two { get; set; }
        public int cndStatus { get; set; }
        public int cndActionID { get; set; }
    }
}