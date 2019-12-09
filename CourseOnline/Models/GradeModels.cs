using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class GradeModels : Grade
    {
        public string user_fullname { get; set; }
        public string user_email { get; set; }
        public double Test1 { get; set; }
        public double Test2 { get; set; }
        public double Test3 { get; set; }
        public double Test4 { get; set; }
        public double Test5 { get; set; }
        public double Test6 { get; set; }

        public int coursework_weight { get; set; }
        public float Total { get; set; }
        public string due_date { get; set; }

    }
}