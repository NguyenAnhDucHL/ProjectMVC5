using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class RegistrationListModel : Registration
    {
        public String subject_name { set; get; }
        public String course_name { set; get; }
        public String user_fullname { set; get; }
        public String user_email { set; get; }
    }
}