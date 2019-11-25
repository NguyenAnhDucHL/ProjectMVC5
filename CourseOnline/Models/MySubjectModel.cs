using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class MySubjectModel
    {
        public int subject_id { get; set; }
        public string email { get; set; }
        public string subject_name { get; set; }
        public string subject_brief_info { get; set; }
        public string picture { get; set; }
        public string subject_category { get; set; }

    }
}