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
    }
}