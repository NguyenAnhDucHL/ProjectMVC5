using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class ResultModel : TestResult
    {
        public string batch_name { get; set; }
        public string user_fullname { get; set; }
        public string user_email { get; set; }
        public double grade_user { get; set; }
    }
}