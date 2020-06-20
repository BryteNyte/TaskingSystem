using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskingSystem.Models
{
    public class FeedbackModel
    {
        public int fdbID { get; set; }
        public string fdbFeedBack { get; set; }
        public int? fdbTimeTaken { get; set; }
        public int? fdbRequiredTime { get; set; }
        public int fdbTaskID { get; set; }
        public string fdbDateCreated { get; set; }
        public int fdbCreatedBy { get; set; }
        public string fdbCreatedBy_Name { get; set; }
        public IEnumerable<FeedbackModel> Items { get; set; }
        public int fdbStatusType { get; set; }
        public bool fdbCheckBox { get; set; }
    }
}