using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class ExamConfigModel : ExamConfig
    {
        public string lesson_name { get; set; }
        public string domain_name { get; set; }
    }
}