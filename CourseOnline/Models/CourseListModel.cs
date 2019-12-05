using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class CourseListModel : Course
    {
        public String user_fullname { set; get; }
        public String subject_name { set; get; }
        public string ObjectiveCourse { get; set; }
        public string subject_brief_info { get; set; }
        public string picture { get; set; }
        public string subject_category { get; set; }
        public string user_email { get; set; }
        public string registration_status { get; set; }

    }
}