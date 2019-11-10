using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class PracticeResultModel
    {
        public string subject_name { get; set; }
        public string exam_name { get; set; }
        public string user_fullname { get; set; }
        public string user_email { get; set; }
        public string tested_at { get; set; }
        public int grade { get; set; }
    }
}