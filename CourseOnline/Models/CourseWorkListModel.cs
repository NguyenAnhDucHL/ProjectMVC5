using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class CourseWorkListModel : Coursework
    {
        public String course_name { set; get; }
        public String test_name { set; get; }
        public String user_email { set; get; }
        public String test_code { set; get; }
        public int exam_id { get; set; }
    }
}